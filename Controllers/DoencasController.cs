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
    public class DoencasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoencasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doencas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tdoencas.Include(d => d.CatDoenca);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Doencas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tdoencas == null)
            {
                return NotFound();
            }

            var doenca = await _context.Tdoencas
                .Include(d => d.CatDoenca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doenca == null)
            {
                return NotFound();
            }

            return View(doenca);
        }

        // GET: Doencas/Create
        public IActionResult Create()
        {
            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Nome");
            return View();
        }

        // POST: Doencas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CatDoencaId")] Doenca doenca)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doenca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Id", doenca.CatDoencaId);
            return View(doenca);
        }

        // GET: Doencas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tdoencas == null)
            {
                return NotFound();
            }

            var doenca = await _context.Tdoencas.FindAsync(id);
            if (doenca == null)
            {
                return NotFound();
            }
            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Nome", doenca.CatDoencaId);
            return View(doenca);
        }

        // POST: Doencas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CatDoencaId")] Doenca doenca)
        {
            if (id != doenca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doenca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoencaExists(doenca.Id))
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
            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Id", doenca.CatDoencaId);
            return View(doenca);
        }

        // GET: Doencas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tdoencas == null)
            {
                return NotFound();
            }

            var doenca = await _context.Tdoencas
                .Include(d => d.CatDoenca)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doenca == null)
            {
                return NotFound();
            }

            return View(doenca);
        }

        // POST: Doencas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tdoencas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tdoencas'  is null.");
            }
            var doenca = await _context.Tdoencas.FindAsync(id);
            if (doenca != null)
            {
                _context.Tdoencas.Remove(doenca);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoencaExists(int id)
        {
          return _context.Tdoencas.Any(e => e.Id == id);
        }
    }
}
