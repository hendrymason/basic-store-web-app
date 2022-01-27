using Microsoft.AspNetCore.Mvc;


namespace Hendry_Mason_HW5.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
