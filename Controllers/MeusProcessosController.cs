using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
using Microsoft.AspNetCore.Mvc;

namespace apoio_decisao_medica.Controllers
{
    public class MeusProcessosController : Controller
    {
        private readonly ApplicationDbContext dbpointer;

        public MeusProcessosController(ApplicationDbContext context)
        {
            dbpointer = context;
        }

        public IActionResult Index(string pesquisaAbertos, string pesquisaFechados)
        {
            //provisorio
            int idMedico = 1;
            //enviar a lista de todos os processos abertos do medico
            if (pesquisaAbertos == null)
            {
                List<Processo> processosAbertos = new List<Processo>();
                foreach (var item in dbpointer.Tprocessos.OrderByDescending(p => p.Id))
                {
                    if (item.MedicoId == idMedico)
                    {
                        if (item.DataHoraFecho == null)
                        {
                            Processo p = new Processo();
                            p.Id = item.Id;
                            p.NumeroProcesso = item.NumeroProcesso;
                            p.UtenteId = item.UtenteId;
                            p.MedicoId = item.MedicoId;
                            p.HospitalId = item.HospitalId;
                            p.DataHoraAbertura = item.DataHoraAbertura;
                            p.DoencaId = item.DoencaId;
                            processosAbertos.Add(p);
                        }
                    }
                }
                ViewBag.ABERTOS = processosAbertos.ToList();
            }
            else
            {
                List<Processo> processosAbertos = new List<Processo>();
                foreach (var item in dbpointer.Tprocessos)
                {
                    if (item.MedicoId == idMedico)
                    {
                        if (item.DataHoraFecho == null)
                        {
                            if (item.NumeroProcesso.ToString() == pesquisaAbertos ||
                                item.DataHoraAbertura.ToString() == pesquisaAbertos)
                            {
                                Processo p = new Processo();
                                p.Id = item.Id;
                                p.NumeroProcesso = item.NumeroProcesso;
                                p.UtenteId = item.UtenteId;
                                p.MedicoId = item.MedicoId;
                                p.HospitalId = item.HospitalId;
                                p.DataHoraAbertura = item.DataHoraAbertura;
                                p.DoencaId = item.DoencaId;
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
                List<Processo> processosFechados = new List<Processo>();
                foreach (var item in dbpointer.Tprocessos.OrderByDescending(p => p.Id))
                {
                    if (item.DataHoraFecho != null)
                    {
                        Processo p = new Processo();
                        p.Id = item.Id;
                        p.NumeroProcesso = item.NumeroProcesso;
                        p.UtenteId = item.UtenteId;
                        p.MedicoId = item.MedicoId;
                        p.HospitalId = item.HospitalId;
                        p.DataHoraAbertura = item.DataHoraAbertura;
                        p.DataHoraFecho = item.DataHoraFecho;
                        p.DoencaId = item.DoencaId;
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
                List<Processo> processosFechados = new List<Processo>();
                foreach (var item in dbpointer.Tprocessos)
                {
                    if (item.MedicoId == idMedico)
                    {
                        if (item.DataHoraFecho != null)
                        {
                            if (item.NumeroProcesso.ToString() == pesquisaFechados ||
                                item.DataHoraAbertura.ToString() == pesquisaFechados ||
                                item.DataHoraFecho.ToString() == pesquisaFechados)
                            {
                                Processo p = new Processo();
                                p.Id = item.Id;
                                p.NumeroProcesso = item.NumeroProcesso;
                                p.UtenteId = item.UtenteId;
                                p.MedicoId = item.MedicoId;
                                p.HospitalId = item.HospitalId;
                                p.DataHoraAbertura = item.DataHoraAbertura;
                                p.DoencaId = item.DoencaId;
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
