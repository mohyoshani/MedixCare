using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class PrescriptionController : Controller
    {
        private readonly IRepository<Prescription> _prescriptionRepo;
        private readonly IRepository<Appointment> _appointmentRepo;

        public PrescriptionController(IRepository<Prescription> prescriptionRepo, IRepository<Appointment> appointmentRepo)
        {
            _prescriptionRepo = prescriptionRepo;
            _appointmentRepo = appointmentRepo;
        }

        public async Task<IActionResult> Index()
        {
            var prescriptions = await _prescriptionRepo.GetAllAsync(null, includes: q => q.Include(p => p.Appointment));
            return View(prescriptions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var prescription = await _prescriptionRepo.GetOneAsync(p => p.Id == id, includes: q => q.Include(p => p.Appointment));
            if (prescription is null)
            {
                return NotFound();
            }
            return View(prescription);
        }

        public async Task<IActionResult> Create()
        {
            var appointments = await _appointmentRepo.GetAllAsync(null);
            ViewBag.Appointments = new SelectList(appointments, "Id", "AppointmentDate");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Prescription prescription)
        {
            if (!ModelState.IsValid)
            {
                var appointments = await _appointmentRepo.GetAllAsync(null);
                ViewBag.Appointments = new SelectList(appointments, "Id", "AppointmentDate");
                return View(prescription);
            }

            prescription.CreatedAt = DateTime.UtcNow;
            await _prescriptionRepo.CreateAsync(prescription, default);
            await _prescriptionRepo.CommitChangesAsync();
            TempData["success"] = "Prescription Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var prescription = await _prescriptionRepo.GetOneAsync(p => p.Id == id);
            if (prescription is null)
            {
                return NotFound();
            }

            var appointments = await _appointmentRepo.GetAllAsync(null);
            ViewBag.Appointments = new SelectList(appointments, "Id", "AppointmentDate", prescription.AppointmentId);
            return View(prescription);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Prescription prescription)
        {
            if (id != prescription.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var appointments = await _appointmentRepo.GetAllAsync(null);
                ViewBag.Appointments = new SelectList(appointments, "Id", "AppointmentDate", prescription.AppointmentId);
                return View(prescription);
            }

            _prescriptionRepo.Update(prescription);
            await _prescriptionRepo.CommitChangesAsync();
            TempData["success"] = "Prescription Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var prescription = await _prescriptionRepo.GetOneAsync(p => p.Id == id, includes: q => q.Include(p => p.Appointment));
            if (prescription is null)
            {
                return NotFound();
            }
            return View(prescription);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prescription = await _prescriptionRepo.GetOneAsync(p => p.Id == id);
            if (prescription is not null)
            {
                _prescriptionRepo.Delete(prescription);
                await _prescriptionRepo.CommitChangesAsync();
                TempData["success"] = "Prescription Deleted Successfully";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
