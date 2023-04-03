using apoio_decisao_medica.Data;
using apoio_decisao_medica.Migrations;
using apoio_decisao_medica.Models;
using apoio_decisao_medica.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using NuGet.Packaging.Rules;
using System;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace apoio_decisao_medica.Controllers
{
    public class NovoProcessoController : Controller
    {
        private readonly ApplicationDbContext dbpointer;

        public NovoProcessoController(ApplicationDbContext context)
        {
            dbpointer = context;
        }

        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = dbpointer.Tutilizador
                .Include(u => u.Medico)
                .Single(u => u.Id == idUSer);
            return utilizador;
        }

        public IActionResult CriarProcesso(int idUtente)
        {
            //obter novo numero de processo
            var processo = (dbpointer.Tprocessos.OrderByDescending(x => x.Id).First());
            int nProcesso;
            if (processo.NumeroProcesso.ToString().Substring(0, 4) == DateTime.Today.ToString("yyyy"))
            {
                nProcesso = processo.NumeroProcesso + 1; // nao esquecer de +1
            }
            else
            {
                nProcesso = Convert.ToInt32(DateTime.Today.ToString("yyyy") + "000001");
            }
            

            //inserir novo processo na base de dados
            Processo novo = new Processo();
            novo.NumeroProcesso = nProcesso;
            novo.UtenteId = idUtente;
            novo.MedicoId = UserLogado().MedicoId;
            novo.HospitalId = Convert.ToInt32(HttpContext.Session.GetInt32("local")); //necessario automatizar
            novo.DataHoraAbertura = DateTime.Today.ToString("dd/MM/yyyy");
            dbpointer.Tprocessos.Add(novo);
            dbpointer.SaveChanges();

            return RedirectToAction("Index", new { nProcesso = nProcesso });
        }

        public IActionResult Index(int nProcesso, int idProcesso, int numProcesso, int idCatSint, int sintoma, int sug, 
            int maisDoencas, int IdCatDoenca, int fechar, int decisao, int removerSint, 
            string pesquisaSint, int reabrir)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            if (nProcesso == 0)
            {
                ViewBag.PROC = numProcesso;
                idProcesso = dbpointer.Tprocessos
                    .Where(p => p.NumeroProcesso == numProcesso)
                    .Select(p => p.Id)
                    .Single();
                ViewBag.IDPROCESSO = idProcesso;
            }
            else
            {
                ViewBag.PROC = nProcesso;
                ViewBag.IDPROCESSO = idProcesso;
            }

            //calcular idade
            ViewBag.IDADE = Idade(numProcesso, nProcesso);

            //reabrir processo
            if (reabrir == 1)
            {
                var processo = dbpointer.Tprocessos
                    .Single(p=>p.Id==idProcesso);
                processo.DataHoraFecho = null;
                dbpointer.SaveChanges();
            }


            //Listar as gategorias dos sintomas
            ViewBag.CATSIN = dbpointer.TcatSintomas.ToList();


            //listar os sintomas filtrados pela cat ou pela pesquisa
            if (idCatSint != 0 || pesquisaSint != null)
            {
                ViewBag.FILTROSINT = FiltroSintomas(pesquisaSint,idCatSint);
            }
            else
            {
                ViewBag.FILTROSINT = null;
            }

            //adiciona sintoma ao processo
            if (sintoma != 0)
            {
                var existe = dbpointer.TprocessoSintomas
                    .FirstOrDefault(s=>s.SintomaId== sintoma && s.ProcessoId==idProcesso);
                if (existe == default)
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
            }


            //remove um sintoma da lista
            if (removerSint != 0)
            {
                var registo = dbpointer.TprocessoSintomas
                    .Where(r=>r.SintomaId == removerSint);
                dbpointer.TprocessoSintomas
                    .RemoveRange(registo);
                dbpointer.SaveChanges();
            }


            //Lista os sintomas do processo aberto
            if (!ListaSintomas(idProcesso).IsNullOrEmpty())
            {                
                ViewBag.LISTASINT = ListaSintomas(idProcesso);
            }
            else
            {
                ViewBag.LISTASINT = null;
            }
            List<Sintoma> listaSintomas = new(ListaSintomas(idProcesso));

            //testar de há sintomas na tabelas
            if (listaSintomas.Count>0)
            {
                ViewBag.PERMITIR = 1;
            }

            ViewBag.ANCORASU = "sugestao";
            //carrega para a lista doencas as doenças que correspondem com os sintomas
            Dictionary<int, int> doencasPercentagem = new Dictionary<int, int>();
            if (sug == 1)
            {
                ViewBag.TODOSSINTOMAS = dbpointer.TdoencaSintomas
                    .OrderByDescending(p => p.Relevancia)
                    .Include(s => s.Sintoma)
                    .ToList();
                ViewBag.TODOSEXAMES = dbpointer.TdoencaExames
                    .OrderByDescending(p => p.Relevancia)
                    .Include(e => e.Exame)
                    .ToList();

                if (listaSintomas.Count != 0)
                {
                    ViewBag.SUG = sug;

                    foreach (var itemS in listaSintomas)
                    {
                        foreach (var itemD in dbpointer.TdoencaSintomas)
                        {
                            if (itemS.Id == itemD.SintomaId)
                            {
                                if (!doencasPercentagem.ContainsKey(itemD.DoencaId))
                                {
                                    doencasPercentagem[itemD.DoencaId] = 0;
                                }
                            }
                        }
                    }
                    //verifica se ha alguma sugestao
                    if (doencasPercentagem.Count == 0)
                    {
                        ViewBag.VAZIO = "Não existem sugestões para apresentar, escolha uma doença abaixo ou adicione uma nova!";
                        maisDoencas = 1;
                    }
                    else
                    {
                        //se houver doenças na lista verifica a relevancia em relaçao aos sintomas apresentados
                        if (doencasPercentagem != null)
                        {
                            foreach (KeyValuePair<int, int> par in doencasPercentagem)
                            {
                                int total = 0;
                                int contU = 0;
                                foreach (var item in dbpointer.TdoencaSintomas)
                                {
                                    if (par.Key == item.DoencaId)
                                    {
                                        total++;
                                        foreach (var itemS in listaSintomas)
                                        {
                                            if (itemS.Id == item.SintomaId)
                                            {
                                                contU++;
                                            }
                                        }
                                    }
                                }
                                doencasPercentagem[par.Key] = (100 * contU) / total;
                            }
                            //verifica qual a doença que se repete mais vezes
                            KeyValuePair<int, int> primeiroPar = doencasPercentagem.First();

                            int maior = primeiroPar.Value;
                            foreach (KeyValuePair<int, int> par in doencasPercentagem)
                            {
                                if (par.Value > maior)
                                {
                                    maior = par.Value;
                                }
                            }
                            ViewBag.PERCENTAGEMSINT = maior;

                            //se a maior percentagem for abaixo de 40% apresenta todas as sugestoes
                            List<int> doencasSugeridas = new List<int>();
                            if (maior <= 40)
                            {
                                foreach (KeyValuePair<int, int> par in doencasPercentagem)
                                {
                                    doencasSugeridas.Add(par.Key);
                                }
                            }
                            else
                            {
                                foreach (KeyValuePair<int, int> par in doencasPercentagem)
                                {
                                    if (maior == par.Value)
                                    {
                                        doencasSugeridas.Add(par.Key);
                                    }
                                }

                            }
                            //envia a(s) doença(s) para a view
                            List<Doenca> doencasSugestao1 = new List<Doenca>();
                            foreach (var item in doencasSugeridas)
                            {
                                foreach (var itemD in dbpointer.Tdoencas.Include(d => d.CatDoenca).Include(d => d.DoencaSintoma))
                                {
                                    if (item == itemD.Id)
                                    {
                                        Doenca d = new Doenca();
                                        d.Id = itemD.Id;
                                        d.Nome = itemD.Nome;
                                        d.CatDoenca = itemD.CatDoenca;
                                        doencasSugestao1.Add(d);
                                    }
                                }
                            }
                            ViewBag.SUGESTAO1 = doencasSugestao1;
                        }

                    }
                }
                else
                {
                    ViewBag.ERROSEMSINT = "Ainda não adicionou nenhum sintoma ao processo! Adicione pelo menos um sintoma para " +
                        "obter sugestões!";
                }
            }
            ViewBag.ANCORA = "maisDoencas";
            //carregar todas as doenças se nao quiser usar a sugerida
            if (maisDoencas == 1)
            {
                ViewBag.MAIS = maisDoencas;
                ViewBag.CATDOENCA = new SelectList(dbpointer.TcatDoencas.OrderBy(d => d.Nome), "Id", "Nome");

                if (IdCatDoenca != 0)
                {
                    ViewBag.MAISDOENCAS = dbpointer.Tdoencas
                        .Include(d => d.CatDoenca)
                        .Include(d=> d.DoencaSintoma)
                        .Where(d => d.CatDoencaId == IdCatDoenca);
                }
            }

            //fechar processo
            if (fechar == 1)
            {
                if (decisao != 0)
                {
                    var fecharProcesso = dbpointer.Tprocessos
                        .First(p => p.NumeroProcesso == numProcesso);
                    fecharProcesso.DoencaId = decisao;
                    fecharProcesso.DataHoraFecho = DateTime.Today.ToString("dd/MM/yyyy");
                    dbpointer.SaveChanges();
                    return RedirectToAction("FecharProcesso", new {idProcesso = idProcesso, fechar = 1 });
                }
                else
                {
                    ViewBag.ERRO = "Deve escolher uma doença antes de fechar o processo!";
                }
            }

            return View();
        }

        public int Idade(int numProcesso, int nProcesso)
        {
            var ficha = dbpointer.Tprocessos
                .Include(p => p.Utente)
                .Single(p => p.NumeroProcesso == numProcesso || p.NumeroProcesso == nProcesso);
            ViewBag.UTENTE = ficha;
            DateTime dataNascimento = DateTime.ParseExact(ficha.Utente.DataNascimento, "dd/MM/yyyy", null);
            int idade = DateTime.Today.Year - dataNascimento.Year;
            if (DateTime.Today < dataNascimento.AddYears(idade))
            {
                idade--;
            }
            return idade;
        }

        public List<Sintoma> FiltroSintomas(string pesquisaSint, int idCatSint)
        {
            if (pesquisaSint != null)
            {
                return dbpointer.Tsintomas
                    .Where(s => s.Nome.ToLower().Contains(pesquisaSint.ToLower()))
                    .OrderBy(s => s.Nome).ToList();
            }
            else
            {
                return dbpointer.Tsintomas
                    .Where(s => s.CatSintomaId == idCatSint)
                    .OrderBy(s => s.Nome).ToList();
            }
        }

        public List<Exame> FiltroExames(string pesquisaExam, int idCatExam)
        {
            if (pesquisaExam != null)
            {
                return dbpointer.Texames
                    .Where(e => e.Nome.ToLower().Contains(pesquisaExam.ToLower()))
                    .OrderBy(e => e.Nome)
                    .ToList();
            }
            else
            {
                return dbpointer.Texames
                    .Where(e => e.CatExameId == idCatExam)
                    .OrderBy(e => e.Nome)
                    .ToList();
            }
        }

        public List<Sintoma> ListaSintomas(int idProcesso)
        {
            List<Sintoma> listaSintomas = new();
            foreach (var item in dbpointer.TprocessoSintomas.Include(p => p.Sintoma))
            {
                if (idProcesso == item.ProcessoId)
                {
                    Sintoma s = new Sintoma();
                    s.Id = item.Sintoma.Id;
                    s.Nome = item.Sintoma.Nome;
                    s.CatSintoma = item.Sintoma.CatSintoma;

                    listaSintomas.Add(s);
                }
            }
            return listaSintomas;
        }

        public List<Exame> ListaExames(int idProcesso)
        {
            List<Exame> listaExames = new();
            foreach (var item in dbpointer.TprocessoExames.Include(p => p.Exame))
            {
                if (idProcesso == item.ProcessoId)
                {
                    Exame e = new Exame();
                    e.Id = item.Exame.Id;
                    e.Nome = item.Exame.Nome;
                    e.CatExame = item.Exame.CatExame;
                    listaExames.Add(e);
                }
            }
            return listaExames;
        }

        public IActionResult FecharProcesso(int submeter, int confirmacao, int idProcesso, int idCatExam, string pesquisaExam,
            int exame, int removerExam, int fechar)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            //recalcula relevancia dos sintomas
            if (fechar == 1)
            {

                //var processo = dbpointer.TprocessoSintomas.Where(p => p.ProcessoId == idProcesso).Select(p=>p.SintomaId).ToList();
                var lista = ListaSintomas(idProcesso);
                foreach (var item in lista)
                {
                    List<DoencaSintoma> doencaSintoma = new();
                    Dictionary<int, int> contarSintomas = new();
                    foreach (var itemS in dbpointer.TprocessoSintomas.Include(p => p.Processo).ToList())
                    {
                        if (item.Id == itemS.SintomaId)
                        {
                            if (itemS.Processo.DoencaId != null)
                            {
                                if (contarSintomas.ContainsKey(Convert.ToInt32(itemS.Processo.DoencaId)))
                                {
                                    contarSintomas[Convert.ToInt32(itemS.Processo.DoencaId)]++;
                                }
                                else
                                {
                                    contarSintomas[Convert.ToInt32(itemS.Processo.DoencaId)] = 1;
                                }
                            }
                            int total = contarSintomas.Values.Sum();
                            foreach (KeyValuePair<int, int> par in contarSintomas)
                            {
                                var relevancia = dbpointer.TdoencaSintomas.FirstOrDefault(p => p.SintomaId == itemS.SintomaId && p.DoencaId == par.Key);
                                if (relevancia != null)
                                {
                                    relevancia.Relevancia = (100 * par.Value) / total;
                                    dbpointer.SaveChanges();
                                }
                                else
                                {
                                    DoencaSintoma ds = new DoencaSintoma();
                                    ds.DoencaId = par.Key;
                                    ds.SintomaId = itemS.SintomaId;
                                    ds.Relevancia = (100 * par.Value) / total;
                                    dbpointer.TdoencaSintomas.Add(ds);
                                    dbpointer.SaveChanges();
                                }
                            }
                        }
                    }

                }
            }


            //apresentar os exames para avaliar
            Processo pro = new();
            pro = dbpointer.Tprocessos
                .Include(p => p.Doenca)
                .Single(p => p.Id == idProcesso);
            ViewBag.PROCESSO = pro;

            //Listra as categorias dos exames
            ViewBag.CATEXAM = dbpointer.TcatExames.ToList();

            //listar os exames filtrados pela cat
            if (idCatExam != 0 || pesquisaExam != null)
            {
                ViewBag.FILTROEXAM = FiltroExames(pesquisaExam, idCatExam);
            }
            else
            {
                ViewBag.FILTROEXAM = null;
            }

            //adiciona exame ao processo
            if (exame != 0)
            {
                var existe = dbpointer.TprocessoExames
                    .FirstOrDefault(e => e.ExameId == exame && e.ProcessoId == idProcesso);
                if (existe == default)
                {
                    var tabelaExames = dbpointer.Texames.ToList();
                    foreach (var item in tabelaExames)
                    {
                        if (exame == item.Id)
                        {
                            ProcessoExame e = new ProcessoExame();
                            e.ProcessoId = idProcesso;
                            e.ExameId = item.Id;
                            dbpointer.TprocessoExames.Add(e);
                            dbpointer.SaveChanges();
                        }
                    }
                }
            }

            //remove um exame da lista
            if (removerExam != 0)
            {
                var registo = dbpointer.TprocessoExames
                    .Where(r => r.ExameId == removerExam);
                dbpointer.TprocessoExames
                    .RemoveRange(registo);
                dbpointer.SaveChanges();
            }

            //Lista os exames do processo aberto
            if (!ListaExames(idProcesso).IsNullOrEmpty())
            {
                ViewBag.LISTAEXAM = ListaExames(idProcesso);
            }
            else
            {
                ViewBag.LISTAEXAM = null;
            }

            if (submeter != 0)
            {
                if (ListaExames(idProcesso).Count == 0)
                {
                    ViewBag.CONTINUAR = 1;
                    ViewBag.SUBMETER = 0;
                }
                else
                {
                    confirmacao = 1;
                }
                if (confirmacao == 1)
                {
                    ViewBag.SUBMETER = 1;
                    var listaExames = ListaExames(idProcesso).ToList();
                    foreach (var item in listaExames)
                    {
                        var atualizar = dbpointer.TdoencaExames.SingleOrDefault(d => d.DoencaId == pro.DoencaId && d.ExameId == item.Id);

                        if (atualizar != null)
                        {
                            atualizar.Relevancia = atualizar.Relevancia + 5;
                            dbpointer.SaveChanges();
                        }
                        else
                        {
                            DoencaExame d = new();
                            d.DoencaId = Convert.ToInt16(pro.DoencaId);
                            d.ExameId = item.Id;
                            d.Relevancia = 5;
                            dbpointer.TdoencaExames.Add(d);
                            dbpointer.SaveChanges();
                        }
                    }
                }
            }
            else
            {
                ViewBag.SUBMETER = 0;
            }
            return View();
        }


        //criar novo sintoma
        public IActionResult NovoSintoma(int numProcesso)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            @ViewBag.PROC = numProcesso;
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            ViewData["CatSintomaId"] = new SelectList(dbpointer.TcatSintomas, "Id", "Nome");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovoSintoma([Bind("Id,Nome,CatSintomaId")] Sintoma sintoma, int numProcesso)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                dbpointer.Add(sintoma);
                await dbpointer.SaveChangesAsync();
                return RedirectToAction("Index", new {numProcesso = numProcesso});
            }
            ViewData["CatSintomaId"] = new SelectList(dbpointer.TcatSintomas, "Id", "Id", sintoma.CatSintomaId);
            return View(sintoma);
        }


        //novo exame
        public IActionResult NovoExame(int numProcesso)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            @ViewBag.PROC = numProcesso;
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            ViewData["CatExameId"] = new SelectList(dbpointer.TcatExames, "Id", "Nome");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovoExame([Bind("Id,Nome,CatExameId")] Exame exame, int numProcesso)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                dbpointer.Add(exame);
                await dbpointer.SaveChangesAsync();
                return RedirectToAction("Index", new { numProcesso = numProcesso });
            }
            ViewData["CatExameId"] = new SelectList(dbpointer.TcatExames, "Id", "Id", exame.CatExameId);
            return View(exame);
        }

        //nova doenca
        public IActionResult NovaDoenca(int numProcesso)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            @ViewBag.PROC = numProcesso;
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            ViewData["CatDoencaId"] = new SelectList(dbpointer.TcatDoencas, "Id", "Nome");
            ViewBag.ANCORA = "maisDoencas";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaDoenca([Bind("Id,Nome,CatDoencaId")] Doenca doenca, int numProcesso)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                dbpointer.Add(doenca);
                await dbpointer.SaveChangesAsync();
                ViewBag.ANCORA = "maisDoencas";
                return RedirectToAction("Index", new {maisDoencas = 1, sug = 1, numProcesso = numProcesso });
            }
            ViewData["CatDoencaId"] = new SelectList(dbpointer.TcatDoencas, "Id", "Id", doenca.CatDoencaId);
            return View(doenca);
        }

    }
}
