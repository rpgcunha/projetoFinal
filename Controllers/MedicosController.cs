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
    public class MedicosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = _context.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }


        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            ViewBag.USER = UserLogado();

            return _context.Tmedicos != null ? 
                          View(await _context.Tmedicos.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Tmedicos'  is null.");
        }

        // GET: Medicos/Create
        public IActionResult Create()
        {
            ViewBag.USER = UserLogado();

            return View();
        }

        // POST: Medicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Bi,Nome,UtilizadorId,Especialidade")] Medico medico)
        {
            ViewBag.USER = UserLogado();

            if (ModelState.IsValid)
            {
                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        // GET: Medicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.USER = UserLogado();

            if (id == null || _context.Tmedicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Tmedicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }
            return View(medico);
        }

        // POST: Medicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Bi,Nome,UtilizadorId,Especialidade")] Medico medico)
        {
            ViewBag.USER = UserLogado();

            if (id != medico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.Id))
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
            return View(medico);
        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.USER = UserLogado();

            if (id == null || _context.Tmedicos == null)
            {
                return NotFound();
            }

            var medico = await _context.Tmedicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.USER = UserLogado();

            if (_context.Tmedicos == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tmedicos'  is null.");
            }
            var medico = await _context.Tmedicos.FindAsync(id);
            if (medico != null)
            {
                _context.Tmedicos.Remove(medico);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
          return (_context.Tmedicos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
