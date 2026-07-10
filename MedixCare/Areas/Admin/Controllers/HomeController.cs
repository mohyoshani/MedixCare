using Microsoft.AspNetCore.Mvc;

namespace MedixCare.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
