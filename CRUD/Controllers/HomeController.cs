using Microsoft.AspNetCore.Mvc;

namespace CRUD.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
