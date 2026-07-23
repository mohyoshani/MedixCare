using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.Admin_Role} , {SD.SuperAdmin_Role} , {SD.Doctor_Role}")]
    public class PatientHistoryController : Controller
    {
        private readonly IRepository<PatientHistory> _historyRepo;
        private readonly IRepository<Doctor> _doctorRepo;
        private readonly IRepository<Patient> _patientRepo;

        public PatientHistoryController(IRepository<PatientHistory> historyRepo, IRepository<Doctor> doctorRepo, IRepository<Patient> patientRepo)
        {
            _historyRepo = historyRepo;
            _doctorRepo = doctorRepo;
            _patientRepo = patientRepo;
        }

        public async Task<IActionResult> Index()
        {
            var histories = await _historyRepo.GetAllAsync(null, includes: q => q.Include(h => h.Doctor).Include(h => h.Patient));
            return View(histories);
        }

        public async Task<IActionResult> Details(int id)
        {
            var history = await _historyRepo.GetOneAsync(h => h.Id == id, includes: q => q.Include(h => h.Doctor).Include(h => h.Patient));
            if (history is null)
            {
                return NotFound();
            }
            return View(history);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDropDowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientHistory history)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropDowns();
                return View(history);
            }

            await _historyRepo.CreateAsync(history, default);
            await _historyRepo.CommitChangesAsync();
            TempData["success"] = "Patient History Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var history = await _historyRepo.GetOneAsync(h => h.Id == id);
            if (history is null)
            {
                return NotFound();
            }

            await LoadDropDowns(history.DoctorId, history.PatientId);
            return View(history);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PatientHistory history)
        {
            if (id != history.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadDropDowns(history.DoctorId, history.PatientId);
                return View(history);
            }

            _historyRepo.Update(history);
            await _historyRepo.CommitChangesAsync();
            TempData["success"] = "Patient History Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var history = await _historyRepo.GetOneAsync(h => h.Id == id, includes: q => q.Include(h => h.Doctor).Include(h => h.Patient));
            if (history is null)
            {
                return NotFound();
            }
            return View(history);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var history = await _historyRepo.GetOneAsync(h => h.Id == id);
            if (history is not null)
            {
                _historyRepo.Delete(history);
                await _historyRepo.CommitChangesAsync();
                TempData["success"] = "Patient History Deleted Successfully";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropDowns(int? doctorId = null, int? patientId = null)
        {
            var doctors = await _doctorRepo.GetAllAsync(null);
            var patients = await _patientRepo.GetAllAsync(null);
            ViewBag.Doctors = new SelectList(doctors, "Id", "Name", doctorId);
            ViewBag.Patients = new SelectList(patients, "Id", "Name", patientId);
        }
    }
}
