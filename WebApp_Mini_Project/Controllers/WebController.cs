using Microsoft.AspNetCore.Mvc;

namespace WebApp_Mini_Project.Controllers
{
    public class WebController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
