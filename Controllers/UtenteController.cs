using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
using apoio_decisao_medica.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Metrics;

namespace apoio_decisao_medica.Controllers
{
    public class UtenteController : Controller
    {
        private readonly ApplicationDbContext dbpointer;

        public UtenteController(ApplicationDbContext context)
        {
            dbpointer = context;
        }
        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = dbpointer.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }

        public IActionResult Index(string nome, string numUtente, string dataNasc, string cidade)
        {
            ViewBag.USER = UserLogado();

            //procurar pelo nome de utente
            if (nome != null)
            {
                return View(dbpointer.Tutentes
                    .Where(u=>u.Nome.ToLower().Contains(nome.ToLower())));
            }
            //procurar pelo numero de utente
            if (numUtente != null)
            {
                return View(dbpointer.Tutentes
                    .Where(u=>u.NumeroUtente.ToString() == numUtente));
            }
            //procurar pela Data de nascimento
            if (dataNasc != null)
            {
                return View(dbpointer.Tutentes
                    .Where(u=>u.DataNascimento.Contains(dataNasc)));
            }
            if (cidade != null)
            {
                return View(dbpointer.Tutentes
                    .Where(u=>u.Cidade != null && u.Cidade.ToLower().Contains(cidade.ToLower())));
            }

            return View(dbpointer.Tutentes.ToList());
        }

        public IActionResult Detalhes(int? id, int idProc)
        {
            ViewBag.USER = UserLogado();

            //detalhes do utente
            var utente = dbpointer.Tutentes
                .Single(u => u.Id == id);

            //calcular idade
            DateTime dataNascimento = DateTime.ParseExact(utente.DataNascimento, "dd/MM/yyyy", null);
            int idade = DateTime.Today.Year - dataNascimento.Year;
            if (DateTime.Today < dataNascimento.AddYears(idade))
            {
                idade--;
            }
            ViewBag.UTENTE = utente;
            ViewBag.IDADE = idade;

            //historico de processos do utente
            List<HistoricoProcesso> listaProcessos = new();
            var processos = dbpointer.Tprocessos
                .OrderByDescending(p => p.Id)
                .Include(p => p.Doenca)
                .Include(p => p.Hospital)
                .Include(p => p.Medico);
            foreach (var item in processos)
            {
                if (item.UtenteId == id)
                {
                    HistoricoProcesso p = new HistoricoProcesso();
                    p.Id = item.Id;
                    p.NumProcesso = item.NumeroProcesso;
                    p.DataAbertura = item.DataHoraAbertura;
                    p.DataFecho = item.DataHoraFecho;
                    p.Doenca = item.Doenca?.Nome;
                    p.MedicoId = item.Medico.Id;
                    p.Medico = item.Medico.Nome;
                    p.Hospital = item.Hospital.Nome;
                    listaProcessos.Add(p);
                }
            }

            //quando se clica no mais carrega os sintomas daquele processo
            if (idProc != 0)
            {
                ViewBag.SINTOMAS = Sintomas(idProc);
                ViewBag.LISTA = idProc;
            }
            else
            {
                ViewBag.LISTA = 0;
            }

            return View(listaProcessos);
        }

        public List<string> Sintomas(int idProc)
        {

            return dbpointer.TprocessoSintomas
                    .Include(s => s.Sintoma)
                    .Where(s => s.ProcessoId == idProc)
                    .Select(s => s.Sintoma.Nome).ToList();
        }

        //criar novo utente
        public IActionResult Novo()
        {
            ViewBag.USER = UserLogado();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novo([Bind("Id,NumeroUtente,Nome,DataNascimento,Genero,Cidade")] Utente utente)
        {
            ViewBag.USER = UserLogado();


            if (ModelState.IsValid)
            {
                dbpointer.Add(utente);
                await dbpointer.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }


        //editar dados do utente
        public async Task<IActionResult> Editar(int? id)
        {
            ViewBag.USER = UserLogado();

            if (id == null || dbpointer.Tutentes == null)
            {
                return NotFound();
            }

            var utente = await dbpointer.Tutentes.FindAsync(id);
            if (utente == null)
            {
                return NotFound();
            }
            return View(utente);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("Id,NumeroUtente,Nome,DataNascimento,Genero,Cidade")] Utente utente)
        {
            ViewBag.USER = UserLogado();

            if (id != utente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    dbpointer.Update(utente);
                    await dbpointer.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtenteExists(utente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Detalhes", new { id = id});
            }
            return View(utente);
        }
        private bool UtenteExists(int id)
        {
            return (dbpointer.Tutentes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
