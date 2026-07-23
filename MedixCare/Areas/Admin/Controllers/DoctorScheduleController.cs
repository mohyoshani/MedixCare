using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.Admin_Role} , {SD.SuperAdmin_Role} , {SD.Doctor_Role}")]
    public class DoctorScheduleController : Controller
    {
        private readonly IRepository<DoctorSchedule> _scheduleRepo;
        private readonly IRepository<Doctor> _doctorRepo;

        public DoctorScheduleController(IRepository<DoctorSchedule> scheduleRepo, IRepository<Doctor> doctorRepo)
        {
            _scheduleRepo = scheduleRepo;
            _doctorRepo = doctorRepo;
        }

        public async Task<IActionResult> Index()
        {
            var schedules = await _scheduleRepo.GetAllAsync(null, includes: q => q.Include(s => s.Doctor));
            return View(schedules);
        }

        public async Task<IActionResult> Details(int id)
        {
            var schedule = await _scheduleRepo.GetOneAsync(s => s.Id == id, includes: q => q.Include(s => s.Doctor));
            if (schedule is null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        public async Task<IActionResult> Create()
        {
            var doctors = await _doctorRepo.GetAllAsync(null);
            ViewBag.Doctors = new SelectList(doctors, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorSchedule schedule)
        {
            if (!ModelState.IsValid)
            {
                var doctors = await _doctorRepo.GetAllAsync(null);
                ViewBag.Doctors = new SelectList(doctors, "Id", "Name");
                return View(schedule);
            }

            await _scheduleRepo.CreateAsync(schedule, default);
            await _scheduleRepo.CommitChangesAsync();
            TempData["success"] = "Schedule Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var schedule = await _scheduleRepo.GetOneAsync(s => s.Id == id);
            if (schedule is null)
            {
                return NotFound();
            }

            var doctors = await _doctorRepo.GetAllAsync(null);
            ViewBag.Doctors = new SelectList(doctors, "Id", "Name", schedule.DoctorId);
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DoctorSchedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var doctors = await _doctorRepo.GetAllAsync(null);
                ViewBag.Doctors = new SelectList(doctors, "Id", "Name", schedule.DoctorId);
                return View(schedule);
            }

            _scheduleRepo.Update(schedule);
            await _scheduleRepo.CommitChangesAsync();
            TempData["success"] = "Schedule Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _scheduleRepo.GetOneAsync(s => s.Id == id, includes: q => q.Include(s => s.Doctor));
            if (schedule is null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _scheduleRepo.GetOneAsync(s => s.Id == id);
            if (schedule is not null)
            {
                _scheduleRepo.Delete(schedule);
                await _scheduleRepo.CommitChangesAsync();
                TempData["success"] = "Schedule Deleted Successfully";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
