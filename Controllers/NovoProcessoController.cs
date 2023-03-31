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

        public IActionResult Index(int nProcesso, int idProcesso, int numProcesso, int idCatSint, int idCatExam, 
            int sintoma, int exame, int sug, int maisDoencas, int IdCatDoenca, int fechar, int decisao,
            int removerSint, int removerExam, string pesquisaSint, string pesquisaExam, int reabrir)
        {
            ViewBag.USER = UserLogado();

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
            //Listra as categorias dos exames
            ViewBag.CATEXAM = dbpointer.TcatExames.ToList();


            //listar os sintomas filtrados pela cat ou pela pesquisa
            if (idCatSint != 0 || pesquisaSint != null)
            {
                ViewBag.FILTROSINT = FiltroSintomas(pesquisaSint,idCatSint);
            }
            else
            {
                ViewBag.FILTROSINT = null;
            }

            //listar os exames filtrados pela cat
            if (idCatExam != 0 || pesquisaExam != null)
            {
                ViewBag.FILTROEXAM = FiltroExames(pesquisaExam,idCatExam);
            }
            else
            {
                ViewBag.FILTROEXAM = null;
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


            //remove um sintoma da lista
            if (removerSint != 0)
            {
                var registo = dbpointer.TprocessoSintomas
                    .Where(r=>r.SintomaId == removerSint);
                dbpointer.TprocessoSintomas
                    .RemoveRange(registo);
                dbpointer.SaveChanges();
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
            //Lista os exames do processo aberto
            if (!ListaExames(idProcesso).IsNullOrEmpty())
            {
                ViewBag.LISTAEXAM = ListaExames(idProcesso);
            }
            else
            {
                ViewBag.LISTAEXAM = null;
            }


            //carrega para a lista doencas as doenças que correspondem com os sintomas
            Dictionary<int, int> doencasPercentagem = new Dictionary<int, int>();
            if (sug == 1)
            {
                if (listaSintomas.Count != 0)
                {
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
                        ViewBag.TODOSSINTOMAS = dbpointer.TdoencaSintomas
                            .OrderByDescending(p => p.Relevancia)
                            .Include(s => s.Sintoma)
                            .ToList();
                        ViewBag.TODOSEXAMES = dbpointer.TdoencaExames
                            .OrderByDescending(p => p.Relevancia)
                            .Include(e => e.Exame)
                            .ToList();
                    }
                }
                else
                {
                    ViewBag.ERROSEMSINT = "Ainda não adicionou nenhum sintoma ao processo! Adicione pelo menos um sintoma para " +
                        "obter sugestões!";
                }
            }

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
                    return RedirectToAction("FecharProcesso", new {numProcesso = numProcesso});
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

        public IActionResult FecharProcesso(List<AvaliarExame> selectlistaExames, int submeter, int confirmacao, int numProcesso)
        {
            ViewBag.USER = UserLogado();

            int idProcesso = dbpointer.Tprocessos
                    .Where(p => p.NumeroProcesso == numProcesso)
                    .Select(p => p.Id)
                    .Single();

            var processos = dbpointer.TprocessoSintomas.ToList();
            foreach (var item in processos)
            {
                if (idProcesso == item.ProcessoId)
                {
                    List<DoencaSintoma> doencaSintoma = new();
                    Dictionary<int, int> contarSintomas = new();
                    foreach (var itemS in dbpointer.TprocessoSintomas.Include(p=>p.Processo))
                    {
                        if (item.SintomaId == itemS.SintomaId)
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
                        }
                    }
                    int total = contarSintomas.Values.Sum();

                    foreach (KeyValuePair<int, int> par in contarSintomas)
                    {
                        var relevancia = dbpointer.TdoencaSintomas.FirstOrDefault(p => p.SintomaId == item.SintomaId && p.DoencaId == par.Key);
                        if (relevancia != null)
                        {
                            relevancia.Relevancia = (100 * par.Value) / total;
                            dbpointer.SaveChanges();
                        }
                        else
                        {
                            DoencaSintoma ds = new DoencaSintoma();
                            ds.DoencaId = par.Key;
                            ds.SintomaId = item.SintomaId;
                            ds.Relevancia = (100 * par.Value) / total;
                            dbpointer.TdoencaSintomas.Add(ds);
                            dbpointer.SaveChanges();
                        }
                    }
                }
            }

            //apresentar os exames para avaliar
            Processo p = new();
            p = dbpointer.Tprocessos
                .Include(p => p.Doenca)
                .Single(p => p.Id == idProcesso);
            ViewBag.PROCESSO = p;


            List<AvaliarExame> listaExames = new();
            foreach (var item in dbpointer.TprocessoExames.Include(e=>e.Exame))
            {
                if (item.ProcessoId == p.Id)
                {
                    AvaliarExame e = new();
                    e.Id = item.ExameId;
                    e.Nome = item.Exame.Nome;
                    e.Selecionado = false;
                    listaExames.Add(e);
                }
            }

            //verificar se nao escolheu nenhum para confirmar se deseja continuar
            if (submeter != 0)
            {
                bool continuar = false;
                foreach (var item in selectlistaExames)
                {
                    if (item.Selecionado)
                    {
                        continuar = true;
                        break;
                    }
                }
                if (continuar || confirmacao == 1)
                {
                    //atualizar relevancia dos exames para a doença
                    int i = 0;
                    List<string> teste = new();
                    foreach (var item in selectlistaExames)
                    {
                        if (item.Selecionado)
                        {
                            var updateRelevancia = dbpointer.TdoencaExames
                                .FirstOrDefault(e => e.ExameId == listaExames[i].Id && e.DoencaId == p.DoencaId);
                            if (updateRelevancia != null)
                            {
                                if (updateRelevancia.Relevancia < 100)
                                {
                                    updateRelevancia.Relevancia = updateRelevancia.Relevancia + 5;
                                    dbpointer.SaveChanges();
                                }
                            }
                            else
                            {
                                DoencaExame r = new();
                                r.DoencaId = Convert.ToInt32(p.DoencaId);
                                r.ExameId = listaExames[i].Id;
                                r.Relevancia = 50;
                                dbpointer.TdoencaExames.Add(r);
                                dbpointer.SaveChanges();
                            }
                        }
                        else
                        {
                            var updateRelevancia = dbpointer.TdoencaExames
                                .FirstOrDefault(e => e.ExameId == listaExames[i].Id && e.DoencaId == p.DoencaId);
                            if (updateRelevancia != null)
                            {
                                if (updateRelevancia.Relevancia > 0)
                                {
                                    updateRelevancia.Relevancia = updateRelevancia.Relevancia - 5;
                                    dbpointer.SaveChanges();
                                }
                            }
                        }
                        i++;
                    }
                    ViewBag.SUBMETER = 1;
                }
                else
                {
                    ViewBag.CONTINUAR = 1;
                    ViewBag.SUBMETER = 0;
                }
            }
            else
            {
                ViewBag.SUBMETER = 0;
            }            
            return View(listaExames);
        }


        //criar novo sintoma
        public IActionResult NovoSintoma(int numProcesso)
        {
            ViewBag.USER = UserLogado();

            @ViewBag.PROC = numProcesso;
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            ViewData["CatSintomaId"] = new SelectList(dbpointer.TcatSintomas, "Id", "Nome");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovoSintoma([Bind("Id,Nome,CatSintomaId")] Sintoma sintoma, int numProcesso)
        {
            ViewBag.USER = UserLogado();

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
            ViewBag.USER = UserLogado();

            @ViewBag.PROC = numProcesso;
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            ViewData["CatExameId"] = new SelectList(dbpointer.TcatExames, "Id", "Nome");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovoExame([Bind("Id,Nome,CatExameId")] Exame exame, int numProcesso)
        {
            ViewBag.USER = UserLogado();

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
            ViewBag.USER = UserLogado();

            @ViewBag.PROC = numProcesso;
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            ViewData["CatDoencaId"] = new SelectList(dbpointer.TcatDoencas, "Id", "Nome");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NovaDoenca([Bind("Id,Nome,CatDoencaId")] Doenca doenca, int numProcesso)
        {
            ViewBag.USER = UserLogado();

            if (ModelState.IsValid)
            {
                dbpointer.Add(doenca);
                await dbpointer.SaveChangesAsync();
                return RedirectToAction("Index", new {maisDoencas = 1, sug = 1, numProcesso = numProcesso });
            }
            ViewData["CatDoencaId"] = new SelectList(dbpointer.TcatDoencas, "Id", "Id", doenca.CatDoencaId);
            return View(doenca);
        }

    }
}
