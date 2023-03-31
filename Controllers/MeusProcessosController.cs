using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
using apoio_decisao_medica.ViewsModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apoio_decisao_medica.Controllers
{
    public class MeusProcessosController : Controller
    {
        private readonly ApplicationDbContext dbpointer;

        public MeusProcessosController(ApplicationDbContext context)
        {
            dbpointer = context;
        }

        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = dbpointer.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }


        public IActionResult Index(string pesquisaAbertos, string pesquisaFechados)
        {
            Utilizador user;
            try
            {
                ViewBag.USER = UserLogado();            
                user = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            //provisorio
            int idMedico = UserLogado().MedicoId;
            //enviar a lista de todos os processos abertos do medico
            if (pesquisaAbertos == null)
            {
                List<HistoricoProcesso> processosAbertos = new List<HistoricoProcesso>();
                foreach (var item in dbpointer.Tprocessos.Include(p => p.Doenca).Include(p => p.Medico).Include(p => p.Hospital).Include(p => p.Utente).OrderByDescending(p => p.Id))
                {
                    if (item.MedicoId == idMedico)
                    {
                        if (item.DataHoraFecho == null)
                        {
                            HistoricoProcesso p = new HistoricoProcesso();
                            p.Id = item.Id;
                            p.NumProcesso = item.NumeroProcesso;
                            p.UtenteId = item.UtenteId;
                            p.NomeUtente = item.Utente?.Nome;
                            p.NumeroUtente = item.Utente.NumeroUtente;
                            p.Hospital = item.Hospital.Nome;
                            p.DataAbertura = item.DataHoraAbertura;
                            p.Doenca = item.Doenca?.Nome;
                            processosAbertos.Add(p);
                        }
                    }
                }
                ViewBag.ABERTOS = processosAbertos.ToList();
            }
            else
            {
                List<HistoricoProcesso> processosAbertos = new List<HistoricoProcesso>();
                foreach (var item in dbpointer.Tprocessos.Include(p => p.Doenca).Include(p => p.Medico).Include(p => p.Hospital).Include(p => p.Utente).OrderByDescending(p => p.Id))
                {
                    if (item.MedicoId == idMedico)
                    {
                        if (item.DataHoraFecho == null)
                        {
                            if (item.NumeroProcesso.ToString() == pesquisaAbertos ||
                                item.Utente.Nome.ToUpper().Contains(pesquisaAbertos.ToUpper()) ||
                                item.Utente.NumeroUtente.ToString() == pesquisaAbertos ||
                                item.DataHoraAbertura.ToString().Contains(pesquisaAbertos))
                            {
                                HistoricoProcesso p = new HistoricoProcesso();
                                p.Id = item.Id;
                                p.NumProcesso = item.NumeroProcesso;
                                p.NomeUtente = item.Utente.Nome;
                                p.NumeroUtente = item.Utente.NumeroUtente;
                                p.Hospital = item.Hospital.Nome;
                                p.DataAbertura = item.DataHoraAbertura;
                                p.Doenca = item.Doenca?.Nome;
                                processosAbertos.Add(p);
                            }
                        }
                    }
                }
                ViewBag.ABERTOS = processosAbertos.ToList();
            }

            //enviar a lista dos ultimos 10 processos fechados do medico
            if (pesquisaFechados == null)
            {             
                int contador = 1;
                List<HistoricoProcesso> processosFechados = new List<HistoricoProcesso>();
                foreach (var item in dbpointer.Tprocessos.Include(p => p.Doenca).Include(p => p.Medico).Include(p => p.Hospital).Include(p => p.Utente).OrderByDescending(p => p.Id))
                {
                    if (item.DataHoraFecho != null && user.Medico.Id == item.MedicoId)
                    {
                        HistoricoProcesso p = new HistoricoProcesso();
                        p.Id = item.Id;
                        p.NumProcesso = item.NumeroProcesso;
                        p.UtenteId = item.UtenteId;
                        p.NomeUtente = item.Utente?.Nome;
                        p.NumeroUtente = item.Utente.NumeroUtente;
                        p.Hospital = item.Hospital.Nome;
                        p.DataAbertura = item.DataHoraAbertura;
                        p.DataFecho = item.DataHoraFecho;
                        p.Doenca = item.Doenca?.Nome;
                        processosFechados.Add(p);                   
                        contador++;
                    }
                    if (contador == 10)
                    {
                        break;
                    }
                }
                ViewBag.FECHADOS = processosFechados.ToList();
                return View();
            }
            else
            {
                List<HistoricoProcesso> processosFechados = new List<HistoricoProcesso>();
                foreach (var item in dbpointer.Tprocessos.Include(p => p.Doenca).Include(p => p.Medico).Include(p => p.Hospital).Include(p => p.Utente).OrderByDescending(p => p.Id))
                {
                    if (item.MedicoId == idMedico)
                    {
                        if (item.DataHoraFecho != null)
                        {
                            if (item.NumeroProcesso.ToString() == pesquisaFechados ||
                                item.Utente.Nome.ToUpper().Contains(pesquisaFechados.ToUpper()) ||
                                item.Utente.NumeroUtente.ToString() == pesquisaFechados ||
                                item.DataHoraAbertura.ToString().Contains(pesquisaFechados) ||
                                item.DataHoraFecho.ToString().Contains(pesquisaFechados))
                            {
                                HistoricoProcesso p = new HistoricoProcesso();
                                p.Id = item.Id;
                                p.NumProcesso = item.NumeroProcesso;
                                p.UtenteId = item.UtenteId;
                                p.NomeUtente = item.Utente?.Nome;
                                p.NumeroUtente = item.Utente.NumeroUtente;
                                p.Hospital = item.Hospital.Nome;
                                p.DataAbertura = item.DataHoraAbertura;
                                p.DataFecho = item.DataHoraFecho;
                                p.Doenca = item.Doenca?.Nome;
                                processosFechados.Add(p);
                            }
                        }
                    }
                }
                ViewBag.FECHADOS = processosFechados.ToList();
            }


            return View();
        }
    }
}
