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

        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = dbpointer.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }

        public IActionResult Index(int idProcesso)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

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

            //calcular idade
            ViewBag.IDADE = Idade(idProcesso);

            return View(dbpointer.Tprocessos.Include(p=> p.Utente).Include(p=>p.Doenca).Where(p=>p.Id == idProcesso));
        }

        public int Idade(int idProcesso)
        {
            var ficha = dbpointer.Tprocessos
                .Include(p => p.Utente)
                .Single(p => p.Id == idProcesso);
            ViewBag.UTENTE = ficha;
            DateTime dataNascimento = DateTime.ParseExact(ficha.Utente.DataNascimento, "dd/MM/yyyy", null);
            int idade = DateTime.Today.Year - dataNascimento.Year;
            if (DateTime.Today < dataNascimento.AddYears(idade))
            {
                idade--;
            }
            return idade;
        }

    }
}
