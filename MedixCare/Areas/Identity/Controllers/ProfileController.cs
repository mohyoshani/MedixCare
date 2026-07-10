using Microsoft.AspNetCore.Mvc;

namespace MedixCare.Areas.Identity.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
