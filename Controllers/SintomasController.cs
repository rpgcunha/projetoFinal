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

        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = _context.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }

        // GET: Sintomas
        public async Task<IActionResult> Index(string pesquisa)
        {
            ViewBag.USER = UserLogado();

            if (pesquisa != null)
            {
                var filtro = _context.Tsintomas
                    .Include(m => m.CatSintoma)
                    .Where(m => m.Nome.ToLower().Contains(pesquisa.ToLower()));
                return View(await filtro.ToListAsync());

            }

            var applicationDbContext = _context.Tsintomas.Include(s => s.CatSintoma);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sintomas/Create
        public IActionResult Create()
        {
            ViewBag.USER = UserLogado();

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
            ViewBag.USER = UserLogado();

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
            ViewBag.USER = UserLogado();

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
            ViewBag.USER = UserLogado();

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
            ViewBag.USER = UserLogado();

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
            ViewBag.USER = UserLogado();

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
