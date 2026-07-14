using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class DoctorLeaveController : Controller
    {
        private readonly IRepository<DoctorLeave> _leaveRepo;
        private readonly IRepository<Doctor> _doctorRepo;

        public DoctorLeaveController(IRepository<DoctorLeave> leaveRepo, IRepository<Doctor> doctorRepo)
        {
            _leaveRepo = leaveRepo;
            _doctorRepo = doctorRepo;
        }

        public async Task<IActionResult> Index()
        {
            var leaves = await _leaveRepo.GetAllAsync(null, includes: q => q.Include(l => l.Doctor));
            return View(leaves);
        }

        public async Task<IActionResult> Details(int id)
        {
            var leave = await _leaveRepo.GetOneAsync(l => l.Id == id, includes: q => q.Include(l => l.Doctor));
            if (leave is null)
            {
                return NotFound();
            }
            return View(leave);
        }

        public async Task<IActionResult> Create()
        {
            var doctors = await _doctorRepo.GetAllAsync(null);
            ViewBag.Doctors = new SelectList(doctors, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorLeave leave)
        {
            if (!ModelState.IsValid)
            {
                var doctors = await _doctorRepo.GetAllAsync(null);
                ViewBag.Doctors = new SelectList(doctors, "Id", "Name");
                return View(leave);
            }

            await _leaveRepo.CreateAsync(leave, default);
            await _leaveRepo.CommitChangesAsync();
            TempData["success"] = "Leave Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var leave = await _leaveRepo.GetOneAsync(l => l.Id == id);
            if (leave is null)
            {
                return NotFound();
            }

            var doctors = await _doctorRepo.GetAllAsync(null);
            ViewBag.Doctors = new SelectList(doctors, "Id", "Name", leave.DoctorId);
            return View(leave);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorLeave leave)
        {
            if (id != leave.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var doctors = await _doctorRepo.GetAllAsync(null);
                ViewBag.Doctors = new SelectList(doctors, "Id", "Name", leave.DoctorId);
                return View(leave);
            }

            _leaveRepo.Update(leave);
            await _leaveRepo.CommitChangesAsync();
            TempData["success"] = "Leave Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var leave = await _leaveRepo.GetOneAsync(l => l.Id == id, includes: q => q.Include(l => l.Doctor));
            if (leave is null)
            {
                return NotFound();
            }
            return View(leave);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leave = await _leaveRepo.GetOneAsync(l => l.Id == id);
            if (leave is not null)
            {
                _leaveRepo.Delete(leave);
                await _leaveRepo.CommitChangesAsync();
                TempData["success"] = "Leave Deleted Successfully";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
