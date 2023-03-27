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

		public IActionResult Index(int change, string antiga, string nova, string nova2)
        {
            ViewBag.USER = UserLogado();

            if (change == 1)
            {
                ViewBag.CHANGE = 1;
            }

            if (change == 2)
            {
                if (antiga == null || nova == null || nova2 == null)
                {
                    ViewBag.ERRO = "Tem de preencher os 3 campos!";
                    ViewBag.CHANGE = 1;
                }
                else
                {
                    if (nova != nova2)
                    {
                        ViewBag.ERRO = "A nova password não corresponde, tente novamente";
                        ViewBag.CHANGE = 1;
                    }
                    else
                    {
                        var utilizador = dbpointer.Tutilizador.Single(p => p.Id == UserLogado().Id);
                        utilizador.Pass = nova;
                        dbpointer.SaveChanges();
                        ViewBag.SUCESS = "Password alterada com sucesso!";
                    }
                }


            }
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
            else
            {
                if (user != null || pass != null)
                {
					ViewBag.ERRO = "Username ou Password incorretos!";
				}
			}
            return View();
        }
    }
}