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
    public class HospitaisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HospitaisController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hospitais
        public async Task<IActionResult> Index()
        {
              return _context.Thospitais != null ? 
                          View(await _context.Thospitais.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Thospitais'  is null.");
        }

        // GET: Hospitais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Thospitais == null)
            {
                return NotFound();
            }

            var hospital = await _context.Thospitais
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // GET: Hospitais/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hospitais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cidade")] Hospital hospital)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hospital);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hospital);
        }

        // GET: Hospitais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Thospitais == null)
            {
                return NotFound();
            }

            var hospital = await _context.Thospitais.FindAsync(id);
            if (hospital == null)
            {
                return NotFound();
            }
            return View(hospital);
        }

        // POST: Hospitais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cidade")] Hospital hospital)
        {
            if (id != hospital.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hospital);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HospitalExists(hospital.Id))
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
            return View(hospital);
        }

        // GET: Hospitais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Thospitais == null)
            {
                return NotFound();
            }

            var hospital = await _context.Thospitais
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hospital == null)
            {
                return NotFound();
            }

            return View(hospital);
        }

        // POST: Hospitais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Thospitais == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Thospitais'  is null.");
            }
            var hospital = await _context.Thospitais.FindAsync(id);
            if (hospital != null)
            {
                _context.Thospitais.Remove(hospital);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HospitalExists(int id)
        {
          return (_context.Thospitais?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
