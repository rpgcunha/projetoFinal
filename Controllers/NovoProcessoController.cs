using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index(string catSint, string sintoma)
        {
            //ir buscar o ultimo registo dos processos e verificar o numero
            //acrescentar mais 1 para o novo processo

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
                        CatSintoma s = new CatSintoma();
                        s.Id = item.Id;
                        s.Nome = item.Nome;
                        sintomasTemp.Add(s);
                    }
                }
                ViewBag.SINTOMAS = sintomasTemp.ToList();
            }
            else
            {
                ViewBag.SINTOMAS = "";
            }

            //adiciona o sintoma à tabela processo/sintoma
            List<CatSintoma> listaSintomas = new List<CatSintoma>();
            foreach (var item in dbpointer.Tsintomas)
            {
                if (sintoma == item.Id.ToString())
                {
                    CatSintoma s = new CatSintoma();
                    s.Id = item.Id;
                    s.Nome = item.Nome;
                    listaSintomas.Add(s);
                }
            }
            return View();
        }
    }
}
