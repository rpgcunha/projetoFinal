using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
using apoio_decisao_medica.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace apoio_decisao_medica.Controllers
{
    public class NovoProcessoController : Controller
    {
        private readonly ApplicationDbContext dbpointer;

        public NovoProcessoController(ApplicationDbContext context)
        {
            dbpointer = context;
        }
        public IActionResult Index(string utente)
        {
            //ir buscar o ultimo registo dos processos e verificar o numero
            //acrescentar mais 1 para o novo processo

            //carregar utentes na drop
            ViewBag.UTENTES = new SelectList(dbpointer.Tutentes, "Id", "Nome");

            //enviar os dados do utente selecionado
            Utente u = new Utente();
            if (!utente.IsNullOrEmpty())
            {
                foreach (var item in dbpointer.Tutentes)
                {
                    if (utente == item.Id.ToString())
                    {
                        u.Id = item.Id;
                        u.NumeroUtente = item.NumeroUtente;
                        u.Nome = item.Nome;
                        u.DataNascimento = item.DataNascimento;
                        u.Genero = item.Genero;
                        u.Cidade = item.Cidade;
                    }
                }
            }
            return View(u);
        }
        public IActionResult Processo(string catSint, string sintoma)
        { 
        //envia as categorias dos sintomas para a drop
        ViewBag.CATSINT = new SelectList(dbpointer.TcatSintomas, "Id", "Nome");            

            //cria uma lista com os sintoma da cat escolhida na drop
            if (!catSint.IsNullOrEmpty())
            {
                List<CatSintoma> sintomasTemp = new List<CatSintoma>();
                foreach (var item in dbpointer.Tsintomas)
                {
                    if (catSint == item.CatSintomaId.ToString())
                    {
                        CatSintoma sin = new CatSintoma();
                        sin.Id = item.Id;
                        sin.Nome = item.Nome;
                        sintomasTemp.Add(sin);
                    }
                }
                ViewBag.SINTOMAS = sintomasTemp.ToList();
            }
            else
            {
                ViewBag.SINTOMAS = "";
            }
            //adiciona o sintoma à tabela processo/sintoma
            var sintomas = dbpointer.Tsintomas.ToList();
            foreach (var item in sintomas)
            {
                if (sintoma == item.Id.ToString())
                {
                    ProcessoSintoma s = new ProcessoSintoma();
                    s.ProcessoId = 1;
                    s.SintomaId = item.Id;
                    dbpointer.TprocessoSintomas.Add(s);
                    dbpointer.SaveChanges();
                }
            }
            //Lista os sintomas do processo aberto
            List<string> listaSintomas = new List<string>();
            foreach (var item in dbpointer.TprocessoSintomas)
            {
                if (1 == item.ProcessoId)
                {
                    listaSintomas.Add(item.Sintoma.Nome);
                }
            }
            if (!listaSintomas.IsNullOrEmpty())
            {
                ViewBag.LISTASINT = listaSintomas.ToList();
            }
            else
            {
                ViewBag.LISTASINT = "";
            }

            return View();
        }
    }
}
