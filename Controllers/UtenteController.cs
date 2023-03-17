using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
using Microsoft.AspNetCore.Mvc;

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
            Utente u = new Utente();
            foreach (var item in dbpointer.Tutentes)
            {
                if (Id == item.Id)
                {
                    u.Id = item.Id;
                    u.Nome = item.Nome;
                    u.NumeroUtente = item.NumeroUtente;
                    u.DataNascimento = item.DataNascimento;
                    u.Genero = item.Genero;
                    u.Cidade = item.Cidade;
                }
            }
            return View(u);
        }
    }
}
