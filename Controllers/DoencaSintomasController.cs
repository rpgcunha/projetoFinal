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
    public class DoencaSintomasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoencaSintomasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoencaSintomas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TdoencaSintomas.Include(d => d.Doenca).Include(d => d.Sintoma);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DoencaSintomas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TdoencaSintomas == null)
            {
                return NotFound();
            }

            var doencaSintoma = await _context.TdoencaSintomas
                .Include(d => d.Doenca)
                .Include(d => d.Sintoma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doencaSintoma == null)
            {
                return NotFound();
            }

            return View(doencaSintoma);
        }

        // GET: DoencaSintomas/Create
        public IActionResult Create()
        {
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Nome");
            ViewData["SintomaId"] = new SelectList(_context.Tsintomas, "Id", "Nome");
            return View();
        }

        // POST: DoencaSintomas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DoencaId,SintomaId,Relevancia")] DoencaSintoma doencaSintoma)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doencaSintoma);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Id", doencaSintoma.DoencaId);
            ViewData["SintomaId"] = new SelectList(_context.Tsintomas, "Id", "Id", doencaSintoma.SintomaId);
            return View(doencaSintoma);
        }

        // GET: DoencaSintomas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TdoencaSintomas == null)
            {
                return NotFound();
            }

            var doencaSintoma = await _context.TdoencaSintomas.FindAsync(id);
            if (doencaSintoma == null)
            {
                return NotFound();
            }
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Nome", doencaSintoma.DoencaId);
            ViewData["SintomaId"] = new SelectList(_context.Tsintomas, "Id", "Nome", doencaSintoma.SintomaId);
            return View(doencaSintoma);
        }

        // POST: DoencaSintomas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DoencaId,SintomaId,Relevancia")] DoencaSintoma doencaSintoma)
        {
            if (id != doencaSintoma.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doencaSintoma);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoencaSintomaExists(doencaSintoma.Id))
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
            ViewData["DoencaId"] = new SelectList(_context.Tdoencas, "Id", "Id", doencaSintoma.DoencaId);
            ViewData["SintomaId"] = new SelectList(_context.Tsintomas, "Id", "Id", doencaSintoma.SintomaId);
            return View(doencaSintoma);
        }

        // GET: DoencaSintomas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TdoencaSintomas == null)
            {
                return NotFound();
            }

            var doencaSintoma = await _context.TdoencaSintomas
                .Include(d => d.Doenca)
                .Include(d => d.Sintoma)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (doencaSintoma == null)
            {
                return NotFound();
            }

            return View(doencaSintoma);
        }

        // POST: DoencaSintomas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TdoencaSintomas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TdoencaSintomas'  is null.");
            }
            var doencaSintoma = await _context.TdoencaSintomas.FindAsync(id);
            if (doencaSintoma != null)
            {
                _context.TdoencaSintomas.Remove(doencaSintoma);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoencaSintomaExists(int id)
        {
          return (_context.TdoencaSintomas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
