using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $" {SD.Admin_Role} , {SD.SuperAdmin_Role} , {SD.Doctor_Role}")]
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

        // 🟢 1. تعديل الـ GET ليستقبل id الروشتة الممرر من زرار Add Items
        [HttpGet]
        public async Task<IActionResult> Create(int id = 0)
        {
            var prescriptions = await _prescriptionRepo.GetAllAsync(null);
            ViewBag.Prescriptions = new SelectList(prescriptions, "Id", "Diagnosis", id);

            var model = new PrescriptionItem();
            if (id != 0)
            {
                model.PrescriptionId = id; // ربط الـ Item بالروشتة المحددة
            }

            return View(model);
        }

        // 🟢 2. تعديل الـ POST لحفظ العنصر وتفادي فشل الـ Validation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PrescriptionItem item)
        {
            // 💡 أهم خطوة: إزالة الخاصية المرجعية من الـ Validation
            ModelState.Remove("Prescription");

            if (!ModelState.IsValid)
            {
                var prescriptions = await _prescriptionRepo.GetAllAsync(null);
                ViewBag.Prescriptions = new SelectList(prescriptions, "Id", "Diagnosis", item.PrescriptionId);
                return View(item);
            }

            await _itemRepo.CreateAsync(item, default);
            await _itemRepo.CommitChangesAsync(); // 👈 الآن سيصل للسطر ده ويسمّع في الداتابيز

            TempData["success"] = "Prescription Item Created Successfully";

            // إعادة التوجيه لصفحة الروشتات الرئيسية بعد الحفظ بنجاح
            return RedirectToAction("Index", "Prescription");
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

            ModelState.Remove("Prescription");

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