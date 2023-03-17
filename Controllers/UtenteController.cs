using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
using apoio_decisao_medica.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
            }
            return View(listaUtentes);
        }

        public IActionResult Detalhes(int? Id)
        {
            //detalhes do utente
            foreach (var item in dbpointer.Tutentes)
            {
                if (Id == item.Id)
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
            List<Processo> listaProcessos = new List<Processo>();
            int nPro = 0;
            foreach (var item in dbpointer.Tprocessos)
            {
                if (item.UtenteId == Id)
                {
                    Processo p = new Processo();
                    nPro = item.Id;
                    p.Id = item.Id;
                    p.NumeroProcesso = item.NumeroProcesso;
                    p.DataHoraAbertura = item.DataHoraAbertura;
                    p.DataHoraFecho = item.DataHoraFecho;
                    p.DoencaId = item.DoencaId;
                    p.MedicoId = item.MedicoId;
                    p.HospitalId = item.HospitalId;
                    listaProcessos.Add(p);
                }
            }
            ViewBag.ID = Id;
            List<int> sintomas = new List<int>();
            foreach (var item in dbpointer.TprocessoSintomas)
            {
                if (nPro == item.ProcessoId)
                {
                    sintomas.Add(item.SintomaId);
                }
            }
            ViewBag.SINTOMAS = sintomas.ToList();
            return View(dbpointer.Tprocessos.ToList());
        }
    }
}
