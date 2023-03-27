using apoio_decisao_medica.Data;
using apoio_decisao_medica.Migrations;
using apoio_decisao_medica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Reflection;

namespace apoio_decisao_medica.Controllers
{
    public class HomeController : Controller
    {
        Utilizador userLogado = new Utilizador();

        private readonly ApplicationDbContext dbpointer;

        public HomeController(ApplicationDbContext context)
        {
            dbpointer = context;
        }

        public Utilizador UserLogado()
        {
			int? idUSer = HttpContext.Session.GetInt32("idUser");
			var utilizador = dbpointer.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
			return utilizador;
		}

		public IActionResult Index()
        {
            ViewBag.USER = UserLogado();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Login(string user, string pass)
        {
            HttpContext.Session.SetInt32("idUser", 0);
            var utilizador = dbpointer.Tutilizador.SingleOrDefault(u => u.Pass == pass && u.User == user);

            if (utilizador != null)
            {                
                HttpContext.Session.SetInt32("idUser", utilizador.Id);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}