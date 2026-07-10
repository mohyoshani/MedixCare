using Microsoft.AspNetCore.Mvc;

namespace MedixCare.Areas.Customer.Controllers
{
    [Area(SD.CUSTOMER_AREA)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
