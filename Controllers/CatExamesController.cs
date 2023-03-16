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
    public class CatExamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatExamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CatExames
        public async Task<IActionResult> Index()
        {
              return _context.TcatExames != null ? 
                          View(await _context.TcatExames.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.TcatExames'  is null.");
        }

        // GET: CatExames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TcatExames == null)
            {
                return NotFound();
            }

            var catExame = await _context.TcatExames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catExame == null)
            {
                return NotFound();
            }

            return View(catExame);
        }

        // GET: CatExames/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CatExames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] CatExame catExame)
        {
            if (ModelState.IsValid)
            {
                _context.Add(catExame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(catExame);
        }

        // GET: CatExames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TcatExames == null)
            {
                return NotFound();
            }

            var catExame = await _context.TcatExames.FindAsync(id);
            if (catExame == null)
            {
                return NotFound();
            }
            return View(catExame);
        }

        // POST: CatExames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] CatExame catExame)
        {
            if (id != catExame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catExame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatExameExists(catExame.Id))
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
            return View(catExame);
        }

        // GET: CatExames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TcatExames == null)
            {
                return NotFound();
            }

            var catExame = await _context.TcatExames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catExame == null)
            {
                return NotFound();
            }

            return View(catExame);
        }

        // POST: CatExames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TcatExames == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TcatExames'  is null.");
            }
            var catExame = await _context.TcatExames.FindAsync(id);
            if (catExame != null)
            {
                _context.TcatExames.Remove(catExame);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatExameExists(int id)
        {
          return (_context.TcatExames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
