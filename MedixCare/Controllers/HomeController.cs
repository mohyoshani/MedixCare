using MedixCare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MedixCare.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
