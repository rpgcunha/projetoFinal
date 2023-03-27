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
    public class UtentesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtentesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = _context.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }

        // GET: Utentes
        public async Task<IActionResult> Index()
        {
            ViewBag.USER = UserLogado();

            return _context.Tutentes != null ? 
                          View(await _context.Tutentes.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Tutentes'  is null.");
        }


        // GET: Utentes/Create
        public IActionResult Create()
        {
            ViewBag.USER = UserLogado();

            return View();
        }

        // POST: Utentes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumeroUtente,Nome,DataNascimento,Genero,Cidade")] Utente utente)
        {
            ViewBag.USER = UserLogado();

            if (ModelState.IsValid)
            {
                _context.Add(utente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index");
        }

        // GET: Utentes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.USER = UserLogado();

            if (id == null || _context.Tutentes == null)
            {
                return NotFound();
            }

            var utente = await _context.Tutentes.FindAsync(id);
            if (utente == null)
            {
                return NotFound();
            }
            return View(utente);
        }

        // POST: Utentes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroUtente,Nome,DataNascimento,Genero,Cidade")] Utente utente)
        {
            ViewBag.USER = UserLogado();

            if (id != utente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtenteExists(utente.Id))
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
            return View(utente);
        }

        // GET: Utentes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.USER = UserLogado();

            if (id == null || _context.Tutentes == null)
            {
                return NotFound();
            }

            var utente = await _context.Tutentes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utente == null)
            {
                return NotFound();
            }

            return View(utente);
        }

        // POST: Utentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.USER = UserLogado();

            if (_context.Tutentes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tutentes'  is null.");
            }
            var utente = await _context.Tutentes.FindAsync(id);
            if (utente != null)
            {
                _context.Tutentes.Remove(utente);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtenteExists(int id)
        {
          return (_context.Tutentes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
