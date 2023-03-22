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
        public IActionResult Index(string nome, string numUtente, string dataNasc)
        {
            List<Utente> listaUtentes = new List<Utente>();

            //procurar pelo nome de utente
            if (nome != null)
            {
                foreach (var item in dbpointer.Tutentes)
                {
                    if (item.Nome.ToString().ToUpper().Contains(nome.ToUpper()))
                    {
                        Utente u = new Utente();
                        u.Id= item.Id;
                        u.Nome = item.Nome;
                        u.NumeroUtente = item.NumeroUtente;
                        u.DataNascimento= item.DataNascimento;
                        u.Genero= item.Genero;
                        u.Cidade= item.Cidade;
                        listaUtentes.Add(u);
                    }
                }
                return View(listaUtentes);
            }
            //procurar pelo numero de utente
            if (numUtente != null)
            {
                foreach (var item in dbpointer.Tutentes)
                {
                    if (item.NumeroUtente.ToString() == numUtente)
                    {
                        Utente u = new Utente();
                        u.Id = item.Id;
                        u.Nome = item.Nome;
                        u.NumeroUtente = item.NumeroUtente;
                        u.DataNascimento = item.DataNascimento;
                        u.Genero = item.Genero;
                        u.Cidade = item.Cidade;
                        listaUtentes.Add(u);
                    }
                }
                return View(listaUtentes);
            }
            //procurar pelo nome de utente
            if (dataNasc != null)
            {
                foreach (var item in dbpointer.Tutentes)
                {
                    if (item.DataNascimento.ToString() == dataNasc)
                    {
                        Utente u = new Utente();
                        u.Id = item.Id;
                        u.Nome = item.Nome;
                        u.NumeroUtente = item.NumeroUtente;
                        u.DataNascimento = item.DataNascimento;
                        u.Genero = item.Genero;
                        u.Cidade = item.Cidade;
                        listaUtentes.Add(u);
                    }
                }
                return View(listaUtentes);
            }
            return View(dbpointer.Tutentes.ToList());
        }

        public IActionResult Detalhes(int? id, int idProc)
        {
            //detalhes do utente
            foreach (var item in dbpointer.Tutentes)
            {
                if (id == item.Id)
                {
                    Utente u = new Utente();
                    u.Id = item.Id;
                    u.Nome = item.Nome;
                    u.NumeroUtente = item.NumeroUtente;
                    u.DataNascimento = item.DataNascimento;
                    u.Genero = item.Genero;
                    u.Cidade = item.Cidade;
                    ViewBag.UTENTE = u;
                }
            }


            //historico de processos do utente
            List<HistoricoProcesso> listaProcessos = new List<HistoricoProcesso>();
            var processos = dbpointer.Tprocessos.OrderByDescending(p => p.Id).Include(p => p.Doenca).Include(p => p.Hospital).Include(p => p.Medico);
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
                    p.Medico = item.Medico.Nome;
                    p.Hospital = item.Hospital.Nome;
                    listaProcessos.Add(p);
                }
            }

            //quando se clica no mais carrega os sintomas daquele processo
            if (idProc != 0)
            {
                List<string> listaSintomas = new List<string>();
                foreach (var item in dbpointer.TprocessoSintomas.Include(s => s.Sintoma))
                {
                    if (idProc == item.ProcessoId)
                    {
                        listaSintomas.Add(item.Sintoma.Nome);
                    }
                }
                ViewBag.SINTOMAS = listaSintomas.ToList();
                ViewBag.LISTA = idProc;
            }
            else
            {
                ViewBag.LISTA = 0;
            }

            return View(listaProcessos.ToList());
        }

        //criar novo utente
        public IActionResult Novo()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novo([Bind("Id,NumeroUtente,Nome,DataNascimento,Genero,Cidade")] Utente utente)
        {
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
