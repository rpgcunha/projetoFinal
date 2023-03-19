using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apoio_decisao_medica.Controllers
{
    public class ProcessoController : Controller
    {
        private readonly ApplicationDbContext dbpointer;

        public ProcessoController(ApplicationDbContext context)
        {
            dbpointer = context;
        }
        public IActionResult Index(int idProcesso)
        {
            ViewBag.IDPROCESSO = idProcesso;

            List<string> sintomas = new List<string>();
            foreach (var item in dbpointer.TprocessoSintomas.Include(s => s.Sintoma))
            {
                if (idProcesso == item.ProcessoId)
                {
                    sintomas.Add(item.Sintoma.Nome);
                }
            }
            ViewBag.SINTOMAS = sintomas;

            List<string> exames = new List<string>();
            foreach (var item in dbpointer.TprocessoExames.Include(e => e.Exame))
            {
                if (idProcesso == item.ProcessoId)
                {
                    exames.Add(item.Exame.Nome);
                }
            }
            ViewBag.EXAMES = exames;

            return View(dbpointer.Tprocessos.Include(p=> p.Utente));
        }
    }
}
