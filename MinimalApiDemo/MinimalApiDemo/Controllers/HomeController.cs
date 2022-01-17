using Microsoft.AspNetCore.Mvc;

namespace MinimalApiDemo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
