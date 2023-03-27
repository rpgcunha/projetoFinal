using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using apoio_decisao_medica.Data;

namespace apoio_decisao_medica.Models
{
    public class DoencasAdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoencasAdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoencasAdmin
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tdoencas.Include(d => d.CatDoenca);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DoencasAdmin/Details/5
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

        // GET: DoencasAdmin/Create
        public IActionResult Create()
        {
            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Id");
            return View();
        }

        // POST: DoencasAdmin/Create
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

        // GET: DoencasAdmin/Edit/5
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
            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Id", doenca.CatDoencaId);
            return View(doenca);
        }

        // POST: DoencasAdmin/Edit/5
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

        // GET: DoencasAdmin/Delete/5
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

        // POST: DoencasAdmin/Delete/5
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
          return (_context.Tdoencas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
