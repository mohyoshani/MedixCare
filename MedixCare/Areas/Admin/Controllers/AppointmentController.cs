using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class AppointmentController : Controller
    {
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IRepository<Doctor> _doctorRepo;
        private readonly IRepository<Patient> _patientRepo;
        private readonly ILogger<Appointment> _logger;
        public AppointmentController(IRepository<Appointment> appointmentRepo, IRepository<Doctor> doctorRepo, IRepository<Patient> patientRepo, ILogger<Appointment> logger)
        {
            _appointmentRepo = appointmentRepo;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
            _logger = logger;
        }
        public async Task<IActionResult> Index(int page = 1, string? query = null)
        {
            var appointments = await _appointmentRepo.GetAllAsync(
            cancellationToken: default,
            includes: q => q.Include(a => a.Doctor).Include(a => a.Patient),
            Tracked: false);

            //search

            if (!string.IsNullOrWhiteSpace(query))
            {
                var filter = query.Trim();

                appointments = appointments.Where(m => (m?.Doctor != null && m.Doctor.Name.Contains(filter))
                || (m?.Patient != null && m.Patient.Name.Contains(filter)));
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

        public async Task<IActionResult> Create()
        {
            var doctors = await _doctorRepo.GetAllAsync(cancellationToken: default, Tracked: false);
            var patients = await _patientRepo.GetAllAsync(cancellationToken: default, Tracked: false);

            var model = new CreateAppointmentVM
            {
                Doctors = doctors,
                Patients = patients
            };

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateAppointmentVM model , CancellationToken cancellationToken)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var doctors = await _doctorRepo.GetAllAsync(cancellationToken: default, Tracked: false);
        //        var patients = await _patientRepo.GetAllAsync(cancellationToken: default, Tracked: false);

        //        return View(model);
        //    }

        //    await _appointmentRepo.CreateAsync(model.Appointment, cancellationToken);
        //    await _appointmentRepo.CommitChangesAsync(cancellationToken);
        //    TempData["success"] = "Appointment Created Successfully";

        //    return RedirectToAction("Index");
        //}
    }
}
 