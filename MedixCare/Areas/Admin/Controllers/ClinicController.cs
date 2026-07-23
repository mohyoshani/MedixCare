using Microsoft.AspNetCore.Mvc;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class ClinicController : Controller
    {
        private readonly IRepository<Clinic> _clinicRepo;
        private readonly IRepository<Doctor> _doctorRepo;

        public ClinicController(IRepository<Clinic> clinicRepo , IRepository<Doctor> doctorRepo)
        {
            _clinicRepo = clinicRepo;
            _doctorRepo = doctorRepo;
        }

        public async Task<IActionResult> Index()
        {
            var clinics = await _clinicRepo.GetAllAsync(null);
            return View(clinics);
        }

        public async Task<IActionResult> Details(int id)
        {
            var clinic = await _clinicRepo.GetOneAsync(c => c.Id == id);
            if (clinic is null)
            {
                return NotFound();
            }
            ViewBag.doctors = await _doctorRepo.GetAllAsync(c => c.ClinicId == id);
            return View(clinic);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Clinic clinic)
        {
            if (!ModelState.IsValid)
            {
                return View(clinic);
            }

            await _clinicRepo.CreateAsync(clinic, default);
            await _clinicRepo.CommitChangesAsync();
            TempData["success"] = "Clinic Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var clinic = await _clinicRepo.GetOneAsync(c => c.Id == id);
            if (clinic is null)
            {
                return NotFound();
            }
            return View(clinic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Clinic clinic)
        {
            if (id != clinic.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(clinic);
            }

            _clinicRepo.Update(clinic);
            await _clinicRepo.CommitChangesAsync();
            TempData["success"] = "Clinic Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clinic = await _clinicRepo.GetOneAsync(c => c.Id == id);
            if (clinic is null)
            {
                return NotFound();
            }
            return View(clinic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clinic = await _clinicRepo.GetOneAsync(c => c.Id == id);
            if (clinic is not null)
            {
                try
                {
                    _clinicRepo.Delete(clinic);
                    await _clinicRepo.CommitChangesAsync();
                    TempData["success"] = "Clinic Deleted Successfully";
                }
                catch (DbUpdateException)
                {
                    TempData["error"] = "Cannot delete this clinic because it still has doctors linked to it";
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
