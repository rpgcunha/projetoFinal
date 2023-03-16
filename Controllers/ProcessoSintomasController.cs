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
    public class ProcessoSintomasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcessoSintomasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ProcessoSintomas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TprocessoSintomas.Include(p => p.Processo).Include(p => p.Sintoma);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProcessoSintomas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TprocessoSintomas == null)
            {
                return NotFound();
            }

            var processoSintoma = await _context.TprocessoSintomas
                .Include(p => p.Processo)
                .Include(p => p.Sintoma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processoSintoma == null)
            {
                return NotFound();
            }

            return View(processoSintoma);
        }

        // GET: ProcessoSintomas/Create
        public IActionResult Create()
        {
            ViewData["ProcessoId"] = new SelectList(_context.Tprocessos, "Id", "NumeroProcesso");
            ViewData["SintomaId"] = new SelectList(_context.Tsintomas, "Id", "Nome");
            return View();
        }

        // POST: ProcessoSintomas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProcessoId,SintomaId")] ProcessoSintoma processoSintoma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(processoSintoma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProcessoId"] = new SelectList(_context.Tprocessos, "Id", "Id", processoSintoma.ProcessoId);
            ViewData["SintomaId"] = new SelectList(_context.Tsintomas, "Id", "Id", processoSintoma.SintomaId);
            return View(processoSintoma);
        }

        // GET: ProcessoSintomas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TprocessoSintomas == null)
            {
                return NotFound();
            }

            var processoSintoma = await _context.TprocessoSintomas.FindAsync(id);
            if (processoSintoma == null)
            {
                return NotFound();
            }
            ViewData["ProcessoId"] = new SelectList(_context.Tprocessos, "Id", "NumeroProcesso", processoSintoma.ProcessoId);
            ViewData["SintomaId"] = new SelectList(_context.Tsintomas, "Id", "Nome", processoSintoma.SintomaId);
            return View(processoSintoma);
        }

        // POST: ProcessoSintomas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProcessoId,SintomaId")] ProcessoSintoma processoSintoma)
        {
            if (id != processoSintoma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processoSintoma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoSintomaExists(processoSintoma.Id))
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
            ViewData["ProcessoId"] = new SelectList(_context.Tprocessos, "Id", "Id", processoSintoma.ProcessoId);
            ViewData["SintomaId"] = new SelectList(_context.Tsintomas, "Id", "Id", processoSintoma.SintomaId);
            return View(processoSintoma);
        }

        // GET: ProcessoSintomas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TprocessoSintomas == null)
            {
                return NotFound();
            }

            var processoSintoma = await _context.TprocessoSintomas
                .Include(p => p.Processo)
                .Include(p => p.Sintoma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (processoSintoma == null)
            {
                return NotFound();
            }

            return View(processoSintoma);
        }

        // POST: ProcessoSintomas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TprocessoSintomas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TprocessoSintomas'  is null.");
            }
            var processoSintoma = await _context.TprocessoSintomas.FindAsync(id);
            if (processoSintoma != null)
            {
                _context.TprocessoSintomas.Remove(processoSintoma);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessoSintomaExists(int id)
        {
          return (_context.TprocessoSintomas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
