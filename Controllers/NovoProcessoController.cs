﻿using apoio_decisao_medica.Data;
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
            int sintoma, int exame, int sug)
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
            //Lista os sintomas do processo aberto
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

            List<int> doencas = new List<int>();
            if (sug == 1)
            {
                foreach (var itemS in listaSintomas)
                {
                    foreach (var itemD in dbpointer.TdoencaSintomas)
                    {
                        if (itemS.Id == itemD.SintomaId)
                        {
                            if (itemD.Relevancia >= 60)
                            {
                                doencas.Add(itemD.DoencaId);
                            }
                        }
                    }
                }
                ViewBag.TESTE1 = doencas.ToList();
            }
            if (doencas != null)
            {
                List<Contagem> contagem = new List<Contagem>();
                foreach (var itemD in doencas)
                {
                    foreach (var itemC in contagem)
                    {
                        if (itemD == itemC.DoencaId)
                        {

                        }
                    }
                }
            }


            return View();
        }
    }
}
