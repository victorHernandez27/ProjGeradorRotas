using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class EquipesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
