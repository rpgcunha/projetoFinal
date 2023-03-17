using Microsoft.AspNetCore.Mvc;

namespace apoio_decisao_medica.Controllers
{
    public class MeusProcessosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
