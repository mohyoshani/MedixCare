using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class PrescriptionItemController : Controller
    {
        private readonly IRepository<PrescriptionItem> _itemRepo;
        private readonly IRepository<Prescription> _prescriptionRepo;

        public PrescriptionItemController(IRepository<PrescriptionItem> itemRepo, IRepository<Prescription> prescriptionRepo)
        {
            _itemRepo = itemRepo;
            _prescriptionRepo = prescriptionRepo;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _itemRepo.GetAllAsync(null, includes: q => q.Include(i => i.Prescription));
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _itemRepo.GetOneAsync(i => i.Id == id, includes: q => q.Include(i => i.Prescription));
            if (item is null)
            {
                return NotFound();
            }
            return View(item);
        }

        public async Task<IActionResult> Create()
        {
            var prescriptions = await _prescriptionRepo.GetAllAsync(null);
            ViewBag.Prescriptions = new SelectList(prescriptions, "Id", "Diagnosis");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrescriptionItem item)
        {
            if (!ModelState.IsValid)
            {
                var prescriptions = await _prescriptionRepo.GetAllAsync(null);
                ViewBag.Prescriptions = new SelectList(prescriptions, "Id", "Diagnosis");
                return View(item);
            }

            await _itemRepo.CreateAsync(item, default);
            await _itemRepo.CommitChangesAsync();
            TempData["success"] = "Prescription Item Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _itemRepo.GetOneAsync(i => i.Id == id);
            if (item is null)
            {
                return NotFound();
            }

            var prescriptions = await _prescriptionRepo.GetAllAsync(null);
            ViewBag.Prescriptions = new SelectList(prescriptions, "Id", "Diagnosis", item.PrescriptionId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PrescriptionItem item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var prescriptions = await _prescriptionRepo.GetAllAsync(null);
                ViewBag.Prescriptions = new SelectList(prescriptions, "Id", "Diagnosis", item.PrescriptionId);
                return View(item);
            }

            _itemRepo.Update(item);
            await _itemRepo.CommitChangesAsync();
            TempData["success"] = "Prescription Item Updated Successfully";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _itemRepo.GetOneAsync(i => i.Id == id, includes: q => q.Include(i => i.Prescription));
            if (item is null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _itemRepo.GetOneAsync(i => i.Id == id);
            if (item is not null)
            {
                _itemRepo.Delete(item);
                await _itemRepo.CommitChangesAsync();
                TempData["success"] = "Prescription Item Deleted Successfully";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
