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
    public class DoencaExamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoencaExamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoencaExames
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TdoencaExames.Include(d => d.Doenca).Include(d => d.Exame);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DoencaExames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TdoencaExames == null)
            {
                return NotFound();
            }

            var doencaExame = await _context.TdoencaExames
                .Include(d => d.Doenca)
                .Include(d => d.Exame)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doencaExame == null)
            {
                return NotFound();
            }

            return View(doencaExame);
        }

        // GET: DoencaExames/Create
        public IActionResult Create()
        {
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Nome");
            ViewData["ExameId"] = new SelectList(_context.Texames, "Id", "Nome");
            return View();
        }

        // POST: DoencaExames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoencaId,ExameId,Relevancia")] DoencaExame doencaExame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doencaExame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Id", doencaExame.DoencaId);
            ViewData["ExameId"] = new SelectList(_context.Texames, "Id", "Id", doencaExame.ExameId);
            return View(doencaExame);
        }

        // GET: DoencaExames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TdoencaExames == null)
            {
                return NotFound();
            }

            var doencaExame = await _context.TdoencaExames.FindAsync(id);
            if (doencaExame == null)
            {
                return NotFound();
            }
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Nome", doencaExame.DoencaId);
            ViewData["ExameId"] = new SelectList(_context.Texames, "Id", "Nome", doencaExame.ExameId);
            return View(doencaExame);
        }

        // POST: DoencaExames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoencaId,ExameId,Relevancia")] DoencaExame doencaExame)
        {
            if (id != doencaExame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doencaExame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoencaExameExists(doencaExame.Id))
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
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Id", doencaExame.DoencaId);
            ViewData["ExameId"] = new SelectList(_context.Texames, "Id", "Id", doencaExame.ExameId);
            return View(doencaExame);
        }

        // GET: DoencaExames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TdoencaExames == null)
            {
                return NotFound();
            }

            var doencaExame = await _context.TdoencaExames
                .Include(d => d.Doenca)
                .Include(d => d.Exame)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doencaExame == null)
            {
                return NotFound();
            }

            return View(doencaExame);
        }

        // POST: DoencaExames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TdoencaExames == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TdoencaExames'  is null.");
            }
            var doencaExame = await _context.TdoencaExames.FindAsync(id);
            if (doencaExame != null)
            {
                _context.TdoencaExames.Remove(doencaExame);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoencaExameExists(int id)
        {
          return (_context.TdoencaExames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
