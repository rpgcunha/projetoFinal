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
    public class ProcessoExamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessoExamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessoExames
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TprocessoExames.Include(p => p.Exame).Include(p => p.Processo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProcessoExames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TprocessoExames == null)
            {
                return NotFound();
            }

            var processoExame = await _context.TprocessoExames
                .Include(p => p.Exame)
                .Include(p => p.Processo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processoExame == null)
            {
                return NotFound();
            }

            return View(processoExame);
        }

        // GET: ProcessoExames/Create
        public IActionResult Create()
        {
            ViewData["ExameId"] = new SelectList(_context.Texames, "Id", "Nome");
            ViewData["ProcessoId"] = new SelectList(_context.Tprocessos, "Id", "NumeroProcesso");
            return View();
        }

        // POST: ProcessoExames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProcessoId,ExameId")] ProcessoExame processoExame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(processoExame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExameId"] = new SelectList(_context.Texames, "Id", "Id", processoExame.ExameId);
            ViewData["ProcessoId"] = new SelectList(_context.Tprocessos, "Id", "Id", processoExame.ProcessoId);
            return View(processoExame);
        }

        // GET: ProcessoExames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TprocessoExames == null)
            {
                return NotFound();
            }

            var processoExame = await _context.TprocessoExames.FindAsync(id);
            if (processoExame == null)
            {
                return NotFound();
            }
            ViewData["ExameId"] = new SelectList(_context.Texames, "Id", "Nome", processoExame.ExameId);
            ViewData["ProcessoId"] = new SelectList(_context.Tprocessos, "Id", "NumeroProcesso", processoExame.ProcessoId);
            return View(processoExame);
        }

        // POST: ProcessoExames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProcessoId,ExameId")] ProcessoExame processoExame)
        {
            if (id != processoExame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processoExame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoExameExists(processoExame.Id))
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
            ViewData["ExameId"] = new SelectList(_context.Texames, "Id", "Id", processoExame.ExameId);
            ViewData["ProcessoId"] = new SelectList(_context.Tprocessos, "Id", "Id", processoExame.ProcessoId);
            return View(processoExame);
        }

        // GET: ProcessoExames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TprocessoExames == null)
            {
                return NotFound();
            }

            var processoExame = await _context.TprocessoExames
                .Include(p => p.Exame)
                .Include(p => p.Processo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processoExame == null)
            {
                return NotFound();
            }

            return View(processoExame);
        }

        // POST: ProcessoExames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TprocessoExames == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TprocessoExames'  is null.");
            }
            var processoExame = await _context.TprocessoExames.FindAsync(id);
            if (processoExame != null)
            {
                _context.TprocessoExames.Remove(processoExame);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessoExameExists(int id)
        {
          return (_context.TprocessoExames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
