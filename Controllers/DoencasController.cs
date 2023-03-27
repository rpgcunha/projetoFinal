using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;

namespace apoio_decisao_medica.Controllers
{
    public class DoencasController : Controller
    {
        private readonly ApplicationDbContext dbpointer;

        public DoencasController(ApplicationDbContext context)
        {
            dbpointer = context;
        }

        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = dbpointer.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }


        // GET: Doencas
        public async Task<IActionResult> Index(string pesquisa)
        {
            ViewBag.USER = UserLogado();

            ViewBag.TODOSSINTOMAS = dbpointer.TdoencaSintomas
                .Include(s => s.Sintoma)
                .OrderByDescending(r => r.Relevancia);
            ViewBag.TODOSEXAMES = dbpointer.TdoencaExames
                .Include(e => e.Exame)
                .OrderByDescending(r => r.Relevancia);
            ViewBag.TODOSPROCESSOS = dbpointer.Tprocessos;

            ViewBag.TOTAL = dbpointer.Tprocessos.Where(d=>d.DataHoraFecho != null).Count();
            ViewBag.CONT = 0;

            var applicationDbContext = dbpointer.Tdoencas
                .Include(d => d.CatDoenca)
                .OrderBy(d => d.Nome);


            if (pesquisa != null)
            {
                applicationDbContext = (IOrderedQueryable<Doenca>)dbpointer.Tdoencas
                    .Include(d => d.CatDoenca)
                    .OrderBy(d => d.Nome)
                    .Where(d => d.Nome.ToUpper().Contains(pesquisa.ToUpper()));
            }
            
            return View(await applicationDbContext.ToListAsync());
        }
        
    }
}
