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
    public class SintomasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SintomasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sintomas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tsintomas.Include(s => s.CatSintoma);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sintomas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tsintomas == null)
            {
                return NotFound();
            }

            var sintoma = await _context.Tsintomas
                .Include(s => s.CatSintoma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sintoma == null)
            {
                return NotFound();
            }

            return View(sintoma);
        }

        // GET: Sintomas/Create
        public IActionResult Create()
        {
            ViewData["CatSintomaId"] = new SelectList(_context.TcatSintomas, "Id", "Nome");
            return View();
        }

        // POST: Sintomas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CatSintomaId")] Sintoma sintoma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sintoma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatSintomaId"] = new SelectList(_context.TcatSintomas, "Id", "Id", sintoma.CatSintomaId);
            return View(sintoma);
        }

        // GET: Sintomas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tsintomas == null)
            {
                return NotFound();
            }

            var sintoma = await _context.Tsintomas.FindAsync(id);
            if (sintoma == null)
            {
                return NotFound();
            }
            ViewData["CatSintomaId"] = new SelectList(_context.TcatSintomas, "Id", "Nome", sintoma.CatSintomaId);
            return View(sintoma);
        }

        // POST: Sintomas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CatSintomaId")] Sintoma sintoma)
        {
            if (id != sintoma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sintoma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SintomaExists(sintoma.Id))
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
            ViewData["CatSintomaId"] = new SelectList(_context.TcatSintomas, "Id", "Id", sintoma.CatSintomaId);
            return View(sintoma);
        }

        // GET: Sintomas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tsintomas == null)
            {
                return NotFound();
            }

            var sintoma = await _context.Tsintomas
                .Include(s => s.CatSintoma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sintoma == null)
            {
                return NotFound();
            }

            return View(sintoma);
        }

        // POST: Sintomas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tsintomas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tsintomas'  is null.");
            }
            var sintoma = await _context.Tsintomas.FindAsync(id);
            if (sintoma != null)
            {
                _context.Tsintomas.Remove(sintoma);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SintomaExists(int id)
        {
          return (_context.Tsintomas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
