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
    public class CatSintomasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatSintomasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CatSintomas
        public async Task<IActionResult> Index()
        {
              return _context.TcatSintomas != null ? 
                          View(await _context.TcatSintomas.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.TcatSintomas'  is null.");
        }

        // GET: CatSintomas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TcatSintomas == null)
            {
                return NotFound();
            }

            var catSintoma = await _context.TcatSintomas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catSintoma == null)
            {
                return NotFound();
            }

            return View(catSintoma);
        }

        // GET: CatSintomas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CatSintomas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] CatSintoma catSintoma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catSintoma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catSintoma);
        }

        // GET: CatSintomas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TcatSintomas == null)
            {
                return NotFound();
            }

            var catSintoma = await _context.TcatSintomas.FindAsync(id);
            if (catSintoma == null)
            {
                return NotFound();
            }
            return View(catSintoma);
        }

        // POST: CatSintomas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] CatSintoma catSintoma)
        {
            if (id != catSintoma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catSintoma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatSintomaExists(catSintoma.Id))
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
            return View(catSintoma);
        }

        // GET: CatSintomas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TcatSintomas == null)
            {
                return NotFound();
            }

            var catSintoma = await _context.TcatSintomas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catSintoma == null)
            {
                return NotFound();
            }

            return View(catSintoma);
        }

        // POST: CatSintomas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TcatSintomas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TcatSintomas'  is null.");
            }
            var catSintoma = await _context.TcatSintomas.FindAsync(id);
            if (catSintoma != null)
            {
                _context.TcatSintomas.Remove(catSintoma);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatSintomaExists(int id)
        {
          return (_context.TcatSintomas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
