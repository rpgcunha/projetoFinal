using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
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
        public IActionResult Index(int Id, int idCatSint, int idCatExam, int sintoma, int exame)
        {
            //obter novo numero de processo
            var processo = (dbpointer.Tprocessos.OrderByDescending(x => x.Id).First());
            int nProcesso = processo.NumeroProcesso; // nao esquecer de +1
            ViewBag.PROC = nProcesso;
            int idProcesso = dbpointer.Tprocessos
                .Where(p => p.NumeroProcesso == nProcesso)
                .Select(p => p.Id)
                .Single();

            //inserir novo processo na base de dados
            //Processo novo = new Processo();
            //novo.NumeroProcesso = nProcesso;
            //novo.UtenteId = Id;
            //novo.MedicoId = 1; //necessoario automatizar
            //novo.HospitalId = 1; //necessario automatizar
            //novo.DataHoraAbertura = DateTime.Today.ToString("dd/MM/yyyy");
            //dbpointer.Tprocessos.Add(novo);
            //dbpointer.SaveChanges();

            //Listar as gategorias dos sintomas
            ViewBag.CATSIN = dbpointer.TcatSintomas.ToList();
            //Listra as categorias dos exames
            ViewBag.CATEXAM = dbpointer.TcatExames.ToList();


            //listar os sintomas filtrados pela cat
            if (idCatSint != 0)
            {
                List<Sintoma> filtroSintomas = new List<Sintoma>();
                foreach (var item in dbpointer.Tsintomas)
                {
                    if (idCatSint == item.CatSintomaId)
                    {
                        Sintoma s = new Sintoma();
                        s.Id = item.Id;
                        s.Nome = item.Nome;
                        s.CatSintomaId = item.CatSintomaId;
                        filtroSintomas.Add(s);
                    }
                }
                ViewBag.FILTROSINT = filtroSintomas.ToList();
            }
            else
            {
                ViewBag.FILTROSINT = null;
            }
            //listar os exames filtrados pela cat
            if (idCatExam != 0)
            {
                List<Exame> filtroExames = new List<Exame>();
                foreach (var item in dbpointer.Texames)
                {
                    if (idCatExam == item.CatExameId)
                    {
                        Exame e = new Exame();
                        e.Id = item.Id;
                        e.Nome = item.Nome;
                        e.CatExameId = item.CatExameId;
                        filtroExames.Add(e);
                    }
                }
                ViewBag.FILTROEXAM = filtroExames.ToList();
            }
            else
            {
                ViewBag.FILTROEXAM = null;
            }


            //adiciona sintoma ao processo
            if (sintoma != 0)
            {
                var tabelaSintomas = dbpointer.Tsintomas.ToList();
                foreach (var item in tabelaSintomas)
                {
                    if (sintoma == item.Id)
                    {
                        ProcessoSintoma s = new ProcessoSintoma();
                        s.ProcessoId = idProcesso;
                        s.SintomaId = item.Id;
                        dbpointer.TprocessoSintomas.Add(s);
                        dbpointer.SaveChanges();
                    }
                }
            }

            //Lista os sintomas do processo aberto
            List<string> listaSintomas = new List<string>();
            foreach (var item in dbpointer.TprocessoSintomas.Include(p => p.Sintoma))
            {
                if (idProcesso == item.ProcessoId)
                {
                    listaSintomas.Add(item.Sintoma.Nome); // ?????????????????????
                }
            }
            if (!listaSintomas.IsNullOrEmpty())
            {
                ViewBag.LISTASINT = listaSintomas.ToList();
            }
            else
            {
                ViewBag.LISTASINT = "Ainda não foi adicionado nenhum sintoma.";
            }



            return View();
        }
    }
}
