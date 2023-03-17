using Microsoft.AspNetCore.Mvc;

namespace apoio_decisao_medica.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
