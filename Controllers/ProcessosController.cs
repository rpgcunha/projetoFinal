using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;

namespace apoio_decisao_medica.Controllers
{
    public class ProcessosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Processos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tprocessos.Include(p => p.Doenca).Include(p => p.Hospital).Include(p => p.Medico).Include(p => p.Utente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Processos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tprocessos == null)
            {
                return NotFound();
            }

            var processo = await _context.Tprocessos
                .Include(p => p.Doenca)
                .Include(p => p.Hospital)
                .Include(p => p.Medico)
                .Include(p => p.Utente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        // GET: Processos/Create
        public IActionResult Create()
        {
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Nome");
            ViewData["HospitalId"] = new SelectList(_context.Thospitais, "Id", "Nome");
            ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Nome");
            ViewData["UtenteId"] = new SelectList(_context.Tutentes, "Id", "Nome");
            return View();
        }

        // POST: Processos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroProcesso,UtenteId,MedicoId,HospitalId,DataHoraAbertura,DataHoraFecho,DoencaId")] Processo processo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(processo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Id", processo.DoencaId);
            ViewData["HospitalId"] = new SelectList(_context.Thospitais, "Id", "Id", processo.HospitalId);
            ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Id", processo.MedicoId);
            ViewData["UtenteId"] = new SelectList(_context.Tutentes, "Id", "Id", processo.UtenteId);
            return View(processo);
        }

        // GET: Processos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tprocessos == null)
            {
                return NotFound();
            }

            var processo = await _context.Tprocessos.FindAsync(id);
            if (processo == null)
            {
                return NotFound();
            }
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Nome", processo.DoencaId);
            ViewData["HospitalId"] = new SelectList(_context.Thospitais, "Id", "Nome", processo.HospitalId);
            ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Nome", processo.MedicoId);
            ViewData["UtenteId"] = new SelectList(_context.Tutentes, "Id", "Nome", processo.UtenteId);
            return View(processo);
        }

        // POST: Processos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroProcesso,UtenteId,MedicoId,HospitalId,DataHoraAbertura,DataHoraFecho,DoencaId")] Processo processo)
        {
            if (id != processo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoExists(processo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Id", processo.DoencaId);
            ViewData["HospitalId"] = new SelectList(_context.Thospitais, "Id", "Id", processo.HospitalId);
            ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Id", processo.MedicoId);
            ViewData["UtenteId"] = new SelectList(_context.Tutentes, "Id", "Id", processo.UtenteId);
            return View(processo);
        }

        // GET: Processos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tprocessos == null)
            {
                return NotFound();
            }

            var processo = await _context.Tprocessos
                .Include(p => p.Doenca)
                .Include(p => p.Hospital)
                .Include(p => p.Medico)
                .Include(p => p.Utente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processo == null)
            {
                return NotFound();
            }

            return View(processo);
        }

        // POST: Processos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tprocessos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tprocessos'  is null.");
            }
            var processo = await _context.Tprocessos.FindAsync(id);
            if (processo != null)
            {
                _context.Tprocessos.Remove(processo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessoExists(int id)
        {
          return (_context.Tprocessos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
