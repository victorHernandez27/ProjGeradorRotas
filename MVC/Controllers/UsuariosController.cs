using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
