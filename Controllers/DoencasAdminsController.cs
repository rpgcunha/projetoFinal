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
    public class DoencasAdminsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoencasAdminsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = _context.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }


        // GET: DoencasAdmins
        public async Task<IActionResult> Index(string pesquisa)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            if (pesquisa != null)
            {
                var filtro = _context.Tdoencas
                    .Include(m => m.CatDoenca)
                    .Where(m => m.Nome.ToLower().Contains(pesquisa.ToLower()));
                return View(await filtro.ToListAsync());

            }

            var applicationDbContext = _context.Tdoencas.Include(d => d.CatDoenca);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DoencasAdmins/Create
        public IActionResult Create()
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Nome");
            return View();
        }

        // POST: DoencasAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,CatDoencaId")] Doenca doenca)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            if (ModelState.IsValid)
            {
                _context.Add(doenca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Nome", doenca.CatDoencaId);
            return View(doenca);
        }

        // GET: DoencasAdmins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            if (id == null || _context.Tdoencas == null)
            {
                return NotFound();
            }

            var doenca = await _context.Tdoencas.FindAsync(id);
            if (doenca == null)
            {
                return NotFound();
            }
            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Nome", doenca.CatDoencaId);
            return View(doenca);
        }

        // POST: DoencasAdmins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CatDoencaId")] Doenca doenca)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

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
            ViewData["CatDoencaId"] = new SelectList(_context.TcatDoencas, "Id", "Nome", doenca.CatDoencaId);
            return View(doenca);
        }

        // GET: DoencasAdmins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

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

        // POST: DoencasAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

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
