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
        public IActionResult Aberto()
        {
            return View();
        }

        public IActionResult Fechado(int idProcesso)
        {
            int idUtente =0, idSintoma = 0, idExame = 0, numProcesso = 0, idHospital = 0;
            int? idDoenca = 0;
            string abertura = string.Empty, fecho = string.Empty;
            var processos = dbpointer.Tprocessos.Include(p => p.Doenca).Include(p => p.Hospital).Include(p => p.Medico).Include(p => p.Utente); ;
            foreach (var item in processos)
            {
                if (idProcesso == item.Id)
                {
                    numProcesso = item.Id;
                    idUtente = item.UtenteId;
                    idDoenca = item.DoencaId;
                    idHospital = item.HospitalId;
                    abertura = item.DataHoraAbertura;
                    fecho = item.DataHoraFecho;
                }
            }
            ViewBag.PROCESSO = numProcesso;
            ViewBag.ABERTURA = abertura;
            ViewBag.FECHO = fecho;


            Utente utente = new Utente();
            foreach (var item in dbpointer.Tutentes)
            {
                if (idUtente == item.Id)
                {
                    utente.Id = item.Id;
                    utente.NumeroUtente = item.NumeroUtente;
                    utente.Nome = item.Nome;
                    utente.DataNascimento = item.DataNascimento;
                    utente.Genero = item.Genero;
                    utente.Cidade = item.Cidade;
                }
            }
            ViewBag.UTENTE = utente;

            
            return View();
        }
    }
}
