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
    public class ExamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = _context.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }


        // GET: Exames
        public async Task<IActionResult> Index(string pesquisa)
        {
            ViewBag.USER = UserLogado();

            if (pesquisa != null)
            {
                var filtro = _context.Texames
                    .Include(m => m.CatExame)
                    .Where(m => m.Nome.ToLower().Contains(pesquisa.ToLower()));
                return View(await filtro.ToListAsync());

            }

            var applicationDbContext = _context.Texames.Include(e => e.CatExame);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Exames/Create
        public IActionResult Create()
        {
            ViewBag.USER = UserLogado();

            ViewData["CatExameId"] = new SelectList(_context.TcatExames, "Id", "Nome");
            return View();
        }

        // POST: Exames/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CatExameId")] Exame exame)
        {
            ViewBag.USER = UserLogado();

            if (ModelState.IsValid)
            {
                _context.Add(exame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatExameId"] = new SelectList(_context.TcatExames, "Id", "Id", exame.CatExameId);
            return View(exame);
        }

        // GET: Exames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.USER = UserLogado();

            if (id == null || _context.Texames == null)
            {
                return NotFound();
            }

            var exame = await _context.Texames.FindAsync(id);
            if (exame == null)
            {
                return NotFound();
            }
            ViewData["CatExameId"] = new SelectList(_context.TcatExames, "Id", "Nome", exame.CatExameId);
            return View(exame);
        }

        // POST: Exames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CatExameId")] Exame exame)
        {
            ViewBag.USER = UserLogado();

            if (id != exame.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exame);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExameExists(exame.Id))
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
            ViewData["CatExameId"] = new SelectList(_context.TcatExames, "Id", "Id", exame.CatExameId);
            return View(exame);
        }

        // GET: Exames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.USER = UserLogado();

            if (id == null || _context.Texames == null)
            {
                return NotFound();
            }

            var exame = await _context.Texames
                .Include(e => e.CatExame)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exame == null)
            {
                return NotFound();
            }

            return View(exame);
        }

        // POST: Exames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.USER = UserLogado();

            if (_context.Texames == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Texames'  is null.");
            }
            var exame = await _context.Texames.FindAsync(id);
            if (exame != null)
            {
                _context.Texames.Remove(exame);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExameExists(int id)
        {
          return (_context.Texames?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
