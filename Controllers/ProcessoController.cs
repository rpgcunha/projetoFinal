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
            //para voltar à pagina anterior
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();

            //listar sintomas do processo
            List<string> sintomas = new();
            sintomas = dbpointer.TprocessoSintomas
                .Include(s => s.Sintoma)
                .Where(p=>p.ProcessoId==idProcesso)
                .Select(p=>p.Sintoma.Nome).ToList();
            ViewBag.SINTOMAS = sintomas;

            //listar exames do processo
            List<string> exames = new();
            exames = dbpointer.TprocessoExames
                .Include(e => e.Exame)
                .Where(p => p.ProcessoId == idProcesso)
                .Select(p => p.Exame.Nome).ToList();
            ViewBag.EXAMES = exames;

            return View(dbpointer.Tprocessos.Include(p=> p.Utente).Include(p=>p.Doenca).Where(p=>p.Id == idProcesso));
        }
    }
}
