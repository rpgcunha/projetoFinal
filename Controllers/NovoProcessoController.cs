using apoio_decisao_medica.Data;
using apoio_decisao_medica.Migrations;
using apoio_decisao_medica.Models;
using apoio_decisao_medica.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging.Rules;
using System;
using System.Collections.Immutable;

namespace apoio_decisao_medica.Controllers
{
    public class NovoProcessoController : Controller
    {
        private readonly ApplicationDbContext dbpointer;

        public NovoProcessoController(ApplicationDbContext context)
        {
            dbpointer = context;
        }
        public IActionResult CriarProcesso(int idUtente)
        {
            //obter novo numero de processo
            var processo = (dbpointer.Tprocessos.OrderByDescending(x => x.Id).First());
            int nProcesso = processo.NumeroProcesso + 1; // nao esquecer de +1

            //inserir novo processo na base de dados
            Processo novo = new Processo();
            novo.NumeroProcesso = nProcesso;
            novo.UtenteId = idUtente;
            novo.MedicoId = 1; //necessoario automatizar
            novo.HospitalId = 1; //necessario automatizar
            novo.DataHoraAbertura = DateTime.Today.ToString("dd/MM/yyyy");
            dbpointer.Tprocessos.Add(novo);
            dbpointer.SaveChanges();

            return RedirectToAction("Index", new { nProcesso = nProcesso });
        }
        public IActionResult Index(int nProcesso, int idProcesso, int numProcesso, int idCatSint, int idCatExam, 
            int sintoma, int exame, int sug, int maisDoencas, int IdCatDoenca, int fechar, int decisao)
        {
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
            //adiciona exame ao processo
            if (exame != 0)
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


            //Lista os sintomas do processo aberto
            List<Sintoma> listaSintomas = new List<Sintoma>();
            foreach (var item in dbpointer.TprocessoSintomas.Include(p => p.Sintoma))
            {
                if (idProcesso == item.ProcessoId)
                {
                    Sintoma s = new Sintoma();
                    s.Id = item.Sintoma.Id;
                    s.Nome= item.Sintoma.Nome;
                    s.CatSintoma = item.Sintoma.CatSintoma;
                    
                    listaSintomas.Add(s);
                }
            }
            if (!listaSintomas.IsNullOrEmpty())
            {
                ViewBag.LISTASINT = listaSintomas.ToList();
            }
            else
            {
                ViewBag.LISTASINT = null;
            }
            //Lista os exames do processo aberto
            List<Exame> listaExames = new List<Exame>();
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
            if (!listaExames.IsNullOrEmpty())
            {
                ViewBag.LISTAEXAM = listaExames.ToList();
            }
            else
            {
                ViewBag.LISTAEXAM = null;
            }


            //carrega para a lista doencas as doenças que correspondem com os sintomas
            Dictionary<int, int> doencasPercentagem = new Dictionary<int, int>();
            if (sug == 1)
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
                    ViewBag.TODOSSINTOMAS = dbpointer.TdoencaSintomas.OrderByDescending(p=>p.Relevancia).Include(s => s.Sintoma);
                }
            }

            //carregar todas as doenças se nao quiser usar a sugerida
            if (maisDoencas == 1)
            {
                ViewBag.MAIS = maisDoencas;
                ViewBag.CATDOENCA = new SelectList(dbpointer.TcatDoencas.OrderBy(d => d.Nome), "Id", "Nome");

                if (IdCatDoenca != 0)
                {
                    List<Doenca> listaMaisDoencas = new List<Doenca>();
                    foreach (var item in dbpointer.Tdoencas.Include(d => d.CatDoenca).Include(d => d.DoencaSintoma))
                    {
                        if (IdCatDoenca == item.CatDoencaId)
                        {
                            Doenca d = new Doenca();
                            d.Id = item.Id;
                            d.Nome = item.Nome;
                            d.CatDoenca = item.CatDoenca;
                            listaMaisDoencas.Add(d);
                        }
                    }
                    ViewBag.MAISDOENCAS = listaMaisDoencas;
                }
            }

            //fechar processo
            if (fechar == 1)
            {
                if (decisao != 0)
                {
                    var fecharProcesso = dbpointer.Tprocessos.First(p => p.NumeroProcesso == numProcesso);
                    fecharProcesso.DoencaId = decisao;
                    fecharProcesso.DataHoraFecho = DateTime.Today.ToString("dd/MM/yyyy");
                    dbpointer.SaveChanges();
                    return RedirectToAction("FecharProcesso", new {numProcesso = numProcesso});
                }
                else
                {
                    ViewBag.ERRO = "Deve escolher uma patologia antes de fechar o processo!";
                }
            }

            return View();
        }

        public IActionResult FecharProcesso(int numProcesso)
        {
            int idProcesso = dbpointer.Tprocessos
                    .Where(p => p.NumeroProcesso == numProcesso)
                    .Select(p => p.Id)
                    .Single();

            var processos = dbpointer.TprocessoSintomas.ToList();
            foreach (var item in processos)
            {
                if (idProcesso == item.ProcessoId)
                {
                    List<DoencaSintoma> doencaSintoma = new List<DoencaSintoma>();
                    Dictionary<int, int> contarSintomas = new Dictionary<int, int>();
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
            ViewBag.TESTE = "chegou aqui, por aqui avaliçao dos exames";
            return View();
        }
    }
}
