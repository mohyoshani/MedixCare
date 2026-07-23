using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.Admin_Role} , {SD.SuperAdmin_Role}")]
    public class DoctorController : Controller
    {
        private readonly IRepository<Doctor> _doctorRepo;
        private readonly IRepository<Clinic> _clinicRepo;
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        public DoctorController(IRepository<Doctor> doctorRepo, IRepository<Clinic> clinicRepo , IRepository<Appointment> appointmentRepo , UserManager<ApplicationUser> userManager)
        {
            _doctorRepo = doctorRepo;
            _clinicRepo = clinicRepo;
            _appointmentRepo = appointmentRepo;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {
            var doctors = await _doctorRepo.GetAllAsync(null, includes: q => q.Include(d => d.Clinic));
            return View(doctors);
        }

        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorRepo.GetOneAsync(d => d.Id == id, includes: q => q.Include(d => d.Clinic));
            if (doctor is null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        public async Task<IActionResult> Create()
        {
            var doctorUsers = await _userManager.GetUsersInRoleAsync(SD.Doctor_Role);
            var clinics = await _clinicRepo.GetAllAsync(null);
            ViewBag.Clinics = new SelectList(clinics, "Id", "Name");
            ViewBag.EmailList =  new SelectList(doctorUsers, "Email", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Doctor doctor)
        {

            if (!ModelState.IsValid)
            {
                var clinics = await _clinicRepo.GetAllAsync(null);
                ViewBag.Clinics = new SelectList(clinics, "Id", "Name");

                var doctorUsers = await _userManager.GetUsersInRoleAsync("Doctor");
                ViewBag.EmailList = new SelectList(doctorUsers, "Email", "Email", doctor.Email);

                return View(doctor);
            }

            await _doctorRepo.CreateAsync(doctor, default);
            await _doctorRepo.CommitChangesAsync();
            TempData["success"] = "Doctor Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var doctor = await _doctorRepo.GetOneAsync(d => d.Id == id);
            if (doctor is null)
            {
                return NotFound();
            }

            var clinics = await _clinicRepo.GetAllAsync(null);
            ViewBag.Clinics = new SelectList(clinics, "Id", "Name", doctor.ClinicId);
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Doctor doctor)
        {
            if (id != doctor.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var clinics = await _clinicRepo.GetAllAsync(null);
                ViewBag.Clinics = new SelectList(clinics, "Id", "Name", doctor.ClinicId);
                return View(doctor);
            }

            _doctorRepo.Update(doctor);
            await _doctorRepo.CommitChangesAsync();
            TempData["success"] = "Doctor Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorRepo.GetOneAsync(d => d.Id == id, includes: q => q.Include(d => d.Clinic));
            if (doctor is null)
            {
                return NotFound();
            }
            return View(doctor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _doctorRepo.GetOneAsync(d => d.Id == id);
            if (doctor is not null)
            {
                try
                {
                    _doctorRepo.Delete(doctor);
                    await _doctorRepo.CommitChangesAsync();
                    TempData["success"] = "Doctor Deleted Successfully";
                }
                catch (DbUpdateException)
                {
                    TempData["error"] = "Cannot delete this doctor because it still has appointments or patient history linked to it";
                }
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ViewAppointments(int id , CancellationToken cancellationToken = default)
        {
            var doctorAppointment = await _appointmentRepo.GetAllAsync(c=>c.DoctorId == id , cancellationToken , Tracked: false , c => c.Include(a => a.Patient));
            return View(doctorAppointment);
        }
    }
}
