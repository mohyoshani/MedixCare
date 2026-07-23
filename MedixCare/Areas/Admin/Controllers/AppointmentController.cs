using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class AppointmentController : Controller
    {
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IRepository<Doctor> _doctorRepo;
        private readonly IRepository <Patient> _patientRepo;
        private readonly IRepository<DoctorLeave> _doctorLeaveRepo;
        private readonly IRepository<DoctorSchedule> _doctorScheduleRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentController(IRepository<Appointment> appointmentRepo, 
            IRepository<Doctor> doctorRepo, IRepository<Patient> patientRepo,
            ILogger<Appointment> logger , IRepository<DoctorLeave> leaves , IRepository<DoctorSchedule> schedule ,
            UserManager<ApplicationUser> userManager)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
            _doctorLeaveRepo = leaves;
            _doctorScheduleRepo = schedule;
            _userManager = userManager;

        }
        public async Task<IActionResult> Index(int page = 1, string? query = null, CancellationToken cancellationToken = default)
        {


            IEnumerable<Appointment> appointments;

         
            if (User.IsInRole(SD.Doctor_Role))
            {
                var user = await _userManager.GetUserAsync(User);
                if(user is null)
                {
                    return View();
                }
                var doctor = await _doctorRepo.GetOneAsync(
                    filter: d => d.Email == user.Email,
                    cancellationToken: cancellationToken,
                    Tracked: false
                );

                if (doctor == null)
                {
                    return NotFound("Doctor profile not found for this account.");
                }

                    appointments = await _appointmentRepo.GetAllAsync(
                    filter: a => a.DoctorId == doctor.Id,
                    cancellationToken: cancellationToken,
                    Tracked: true,
                    includes: q => q.Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .ThenInclude(a => a!.Appointments));
            }
            else
            {
             
                appointments = await _appointmentRepo.GetAllAsync(
                    filter: null,
                    cancellationToken: cancellationToken,
                    Tracked: true,
                    includes: q => q.Include(a => a.Doctor)
                    .Include(a => a.Patient)
                    .ThenInclude(a => a!.Appointments));
            }

            //search

            if (!string.IsNullOrWhiteSpace(query))
            {
                var filter = query.Trim().ToLower();

                appointments = appointments.Where(m => (m?.Doctor != null && m.Doctor.Name.ToLower().Contains(filter))
                || (m?.Patient != null && m.Patient.Name.ToLower().Contains(filter)));
            }

            //pagination
            var totalAppointments = appointments.Count();
            var totalPages = Math.Ceiling(totalAppointments / 10.00);
            appointments = appointments.Skip((page - 1) * 10).Take(10).ToList();



            var model = new AppointementVM
            {
                query = query ?? string.Empty,
                TotalPages = totalPages,
                CurrentPage = page,
                Appointments = appointments
            };

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Create(int id, CancellationToken cancellationToken = default)
        {
            var patients = await _patientRepo.GetAllAsync(null, cancellationToken, Tracked: false);
            var doctor = await _doctorRepo.GetOneAsync(x => x.Id == id, cancellationToken, false, null);
            if (doctor == null)
            {
                return NotFound();
            }
            var leaves = await _doctorLeaveRepo.GetAllAsync(
         x => x.DoctorId == id && x.LeaveTo >= DateTime.UtcNow,
         cancellationToken,
         Tracked: false);

        
            var schedules = await _doctorScheduleRepo.GetAllAsync(
                x => x.DoctorId == id,
                cancellationToken,
                Tracked: false);
            var model = new CreateAppointmentVM()
            {
                AppointmentDate = DateTime.UtcNow,
                VisitType = VisitType.Examination,
                DoctorId = id,
                DoctorName = doctor.Name,
                PatientsList = patients , 
                doctorLeaves = leaves ,
                doctorSchedules = schedules
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAppointmentVM model, CancellationToken cancellationToken = default)
        {
  
            if (!ModelState.IsValid)
            {

                model.PatientsList = await _patientRepo.GetAllAsync(null, cancellationToken, Tracked: false);

                model.doctorSchedules = await _doctorScheduleRepo.GetAllAsync(
                    x => x.DoctorId == model.DoctorId,
                    cancellationToken,
                    Tracked: false);


                model.doctorLeaves = await _doctorLeaveRepo.GetAllAsync(
                    x => x.DoctorId == model.DoctorId && x.LeaveTo >= DateTime.UtcNow,
                    cancellationToken,
                    Tracked: false);

              
                var doctor = await _doctorRepo.GetOneAsync(x => x.Id == model.DoctorId, cancellationToken, false, null);
                if (doctor is not null)
                {
                    model.DoctorName = doctor.Name;
                }

                return View(model);
            }

          
            var appointment = new Appointment()
            {
                AppointmentDate = model.AppointmentDate,
                PatientId = model.PatientId,
                DoctorId = model.DoctorId,
                VisitType = model.VisitType,
                Status = AppointmentStatus.Scheduled,
                CreatedAt = DateTime.UtcNow
            };

            await _appointmentRepo.CreateAsync(appointment, cancellationToken);
            await _appointmentRepo.CommitChangesAsync(cancellationToken);

            TempData["success"] = "Appointment Created Successfully!";
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Update(int id, CancellationToken cancellationToken = default)
        {

            var appointment = await _appointmentRepo.GetOneAsync(x => x.Id == id, cancellationToken, Tracked: false, null);
            if (appointment is null)
            {
                return NotFound();
            }


            var patients = await _patientRepo.GetAllAsync(null, cancellationToken, Tracked: false);


            var doctor = await _doctorRepo.GetOneAsync(x => x.Id == appointment.DoctorId, cancellationToken, false, null);

            var schedules = await _doctorScheduleRepo.GetAllAsync(x => x.DoctorId == appointment.DoctorId, cancellationToken, Tracked: false);
            var leaves = await _doctorLeaveRepo.GetAllAsync(x => x.DoctorId == appointment.DoctorId && x.LeaveTo >= DateTime.UtcNow, cancellationToken, Tracked: false);


            var model = new UpdateAppointmentVM()
            {
                Id = appointment.Id,
                AppointmentDate = appointment.AppointmentDate,
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                DoctorName = doctor?.Name ?? "Unknown",
                VisitType = appointment.VisitType,
                PatientsList = patients,
                DoctorSchedules = schedules,
                DoctorLeaves = leaves,
                Status = appointment.Status
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateAppointmentVM model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
       
                model.PatientsList = await _patientRepo.GetAllAsync(null, cancellationToken, Tracked: false);
                model.DoctorSchedules = await _doctorScheduleRepo.GetAllAsync(x => x.DoctorId == model.DoctorId, cancellationToken, Tracked: false);
                model.DoctorLeaves = await _doctorLeaveRepo.GetAllAsync(x => x.DoctorId == model.DoctorId && x.LeaveTo >= DateTime.UtcNow, cancellationToken, Tracked: false);

                var doctor = await _doctorRepo.GetOneAsync(x => x.Id == model.DoctorId, cancellationToken, false, null);
                if (doctor != null) model.DoctorName = doctor.Name;

                return View(model);
            }

       
            var appointment = await _appointmentRepo.GetOneAsync(x => x.Id == model.Id, cancellationToken, Tracked: true, null);
            if (appointment is null)
            {
                return NotFound();
            }

            appointment.Status = model.Status;
            appointment.AppointmentDate = model.AppointmentDate;
            appointment.PatientId = model.PatientId;
            appointment.VisitType = model.VisitType;

      
            await _appointmentRepo.CommitChangesAsync(cancellationToken);

            TempData["success"] = "Appointment Updated Successfully!";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> SelectDoctor(int page = 1, string? query = null, CancellationToken cancellationToken = default)
        {
            var doctor = await _doctorRepo.GetAllAsync(null, cancellationToken, Tracked: false, a => a.Include(x => x.Clinic));

            if (!string.IsNullOrEmpty(query))
            {
                var filter = query.ToLower().Trim();
                doctor = doctor.Where(x => x.Name.ToLower().Contains(filter));
            }

            //pagination
            var doctorCount = doctor.Count();
            var totalpages = (int)Math.Ceiling(doctorCount / 10.0);
            doctor = doctor.Skip((page - 1) * 10).Take(10).ToList();

            var model = new DoctorSelectVM()
            {
                CurrentPage = page,
                query = query ?? string.Empty,
                Doctors = doctor,
                TotalPages = totalpages,
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateFollowUp(int parentId, CancellationToken cancellationToken = default)
        {
            var oldAppointment = await _appointmentRepo.GetOneAsync(
                filter: x => x.Id == parentId,
                 cancellationToken: cancellationToken,
                 Tracked: true,
                 includes: q => q.Include(a => a.Patient).Include(a => a.Doctor));

            if(oldAppointment is null)
            {
                return RedirectToAction("Create");
            }

            var existingFollows = await _appointmentRepo.GetAllAsync(x => x.ParentAppointmentId == parentId && x.VisitType == VisitType.FollowUp);
            if(existingFollows.Count() >= 4)
            {
                TempData["error"] = "Patient Exceeded follow Ups";
                return RedirectToAction("Index");
            }
            var model= new CreateFollowUpVM
            {
                patientId = oldAppointment.PatientId,
                doctorId = oldAppointment.DoctorId,
                parentId = parentId,          
                PatientName = oldAppointment.Patient?.Name,  
                DoctorName = oldAppointment.Doctor?.Name,      
                AppointmentDate = DateTime.UtcNow.AddDays(7)  
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFollowUp(CreateFollowUpVM model, CancellationToken cancellationToken = default)
        {
            var oldAppointment = await _appointmentRepo.GetOneAsync(
        filter: x => x.Id == model.parentId,
        cancellationToken: cancellationToken,
        Tracked: false,
        includes: null);
            if(oldAppointment is null)
            {
                return View(model);

            }
            if (!ModelState.IsValid)
            {
                model.PatientName = oldAppointment.Patient?.Name;
                model.DoctorName = oldAppointment.Doctor?.Name;
                return View(model);
            }

            var existingFollows = await _appointmentRepo.GetAllAsync(x => x.ParentAppointmentId == model.parentId && x.VisitType == VisitType.FollowUp);
            
            if (existingFollows.Count() >= 4)
            {
                TempData["error"] = "Patient Exceeded follow Ups";
                return View(model);
            }

            var followUp = new Appointment()
            {
               ParentAppointmentId=model.parentId,
          
                PatientId = model.patientId,
                DoctorId = model.doctorId,
                VisitType = VisitType.FollowUp, 
                Status = AppointmentStatus.Scheduled,

                AppointmentDate = model.AppointmentDate.Kind != DateTimeKind.Utc
                          ? model.AppointmentDate.ToUniversalTime()
                          : model.AppointmentDate,
                CreatedAt = DateTime.UtcNow
            };

            await _appointmentRepo.CreateAsync(followUp, cancellationToken);
            await _appointmentRepo.CommitChangesAsync(cancellationToken);
            TempData["success"] = "Follow up added successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var appointment = await _appointmentRepo.GetOneAsync(
                filter: x => x.Id == id,
                cancellationToken: cancellationToken,
                Tracked: true);

            if (appointment is null)
            {
                return NotFound();
            }

            _appointmentRepo.Delete(appointment);
            await _appointmentRepo.CommitChangesAsync(cancellationToken);

            return RedirectToAction("Index");
        }
    }
}
