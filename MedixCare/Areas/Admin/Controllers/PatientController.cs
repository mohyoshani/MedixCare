using MedixCare.DataAccess.ModelConfigurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
 
    public class PatientController : Controller
    {
        private readonly IRepository<Patient> _patientRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Doctor> _doctorRepo;

        public PatientController(IRepository<Patient> _patient , UserManager<ApplicationUser> userManager , IRepository<Doctor> doctorRepo)
        {
            _patientRepo = _patient;
            _userManager = userManager;
            _doctorRepo = doctorRepo;
        }

        [Authorize(Roles = $"{SD.Employee_Role} , {SD.Admin_Role} , {SD.SuperAdmin_Role} , {SD.Doctor_Role}")]
        public async Task<IActionResult> Index(int page = 1, string? query = null , CancellationToken cancellationToken = default)
        {
            IEnumerable<Patient> patients;
         
            if (User.IsInRole(SD.Doctor_Role))
            {
                var user = await _userManager.GetUserAsync(User);
                if(user is null)
                {
                    return NotFound();
                }
                var doctor = await _doctorRepo.GetOneAsync(
                     filter: d => d.Email == user.Email,
                     cancellationToken: cancellationToken,
                     Tracked: false
                 );
                if(doctor is null)
                {
                    return NotFound();
                }

                patients = await _patientRepo.GetAllAsync(
                    filter: p => p.Appointments!.Any(a => a.DoctorId == doctor.Id),
                    cancellationToken: cancellationToken,
                    Tracked: false
                );
            }

            else
            {
                patients = await _patientRepo.GetAllAsync(
                    filter: null,
                    cancellationToken: cancellationToken,
                    Tracked: false
                );
            }

            //search

            if (!string.IsNullOrEmpty(query))
            {
                var filter = query.Trim().ToLower();
                patients = patients.Where(m => m.Name.ToLower().Contains(filter));
            }

            //pagination 

            var patientsCount = patients.Count();
            var totalPages = Math.Ceiling(patientsCount / 10.0);
            patients = patients.Skip((page - 1) * 10).Take(10).ToList();

            var model = new PatientVM()
            {
                currentPage = page,
                Patients = patients,
                query = query ?? string.Empty,
                totalPages = totalPages,
            };
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.Employee_Role} , {SD.Admin_Role} , {SD.SuperAdmin_Role}")]
        public IActionResult Create()
        {
            var model = new CreatePatientFlattendVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.Employee_Role} , {SD.Admin_Role} , {SD.SuperAdmin_Role}")]
        public async Task<IActionResult> Create(CreatePatientFlattendVM model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Failed To Create Patient";
                return View(model);
            }
            var patient = new Patient()
            {
                Name = model.Name,
                MobileNumber = model.MobileNumber,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _patientRepo.CreateAsync(patient, cancellationToken);
            await _patientRepo.CommitChangesAsync(cancellationToken);
            TempData["success"] = "Patient Created Successfully";

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.Employee_Role} , {SD.Admin_Role} , {SD.SuperAdmin_Role}")]
        public async Task<IActionResult> Update(int id, CancellationToken cancellationToken = default)
        {

            var patient = await _patientRepo.GetOneAsync(p => p.Id == id , cancellationToken);

            if (patient is null)
                return NotFound();
            var model = new UpdatePatientVM()
            {
                Id = patient.Id,
                Name = patient.Name,
                MobileNumber = patient.MobileNumber,
                DateOfBirth = patient.DateOfBirth,
                isActive = patient.IsActive,

            };
            return View(model);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.Employee_Role} , {SD.Admin_Role} , {SD.SuperAdmin_Role}")]
        public async Task<IActionResult> Update(UpdatePatientVM model , CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {

                return View(model);
            }
            var patient = await _patientRepo.GetOneAsync(p => p.Id == model.Id , cancellationToken);
            if(patient is null)
            {
                return NotFound();
            }

                patient.Name = model.Name;
                patient.MobileNumber = model.MobileNumber;
                patient.DateOfBirth = model.DateOfBirth;
                patient.IsActive = model.isActive;
        

            await _patientRepo.CommitChangesAsync(cancellationToken);
            TempData["success"] = "Patient Updated Successfully";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.Employee_Role} , {SD.Admin_Role} , {SD.SuperAdmin_Role}")]
        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken = default )
        {
            var patient = await _patientRepo.GetOneAsync(p=>p.Id == id , cancellationToken);
            if(patient is null)
            {
                return NotFound();
            }
            _patientRepo.Delete(patient);
            await _patientRepo.CommitChangesAsync(cancellationToken);
            TempData["success"] = "Patient Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
