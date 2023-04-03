using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using apoio_decisao_medica.Data;
using apoio_decisao_medica.Models;
using System.Reflection;

namespace apoio_decisao_medica.Controllers
{
    public class CatDoencasController : Controller
    {
        private readonly ApplicationDbContext _context;

		public CatDoencasController(ApplicationDbContext context)
        {
            _context = context;
        }

		public Utilizador UserLogado()
		{
			int? idUSer = HttpContext.Session.GetInt32("idUser");
			var utilizador = _context.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
			return utilizador;
		}

		// GET: CatDoencas
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

            if (!UserLogado().IsAdmin)
            {
                return NotFound();
            }

            if (pesquisa != null)
            {
                var filtro = _context.TcatDoencas.Where(f => f.Nome.ToLower().Contains(pesquisa.ToLower()));
                return View(await filtro.ToListAsync());

            }

            return View(await _context.TcatDoencas.ToListAsync());
        }

        // GET: CatDoencas/Create
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

            if (!UserLogado().IsAdmin)
            {
                return NotFound();
            }

            return View();
        }

        // POST: CatDoencas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] CatDoenca catDoenca)
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
                _context.Add(catDoenca);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            if (!UserLogado().IsAdmin)
            {
                return NotFound();
            }

            return View(catDoenca);
        }

        // GET: CatDoencas/Edit/5
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

            if (!UserLogado().IsAdmin)
            {
                return NotFound();
            }

            if (id == null || _context.TcatDoencas == null)
            {
                return NotFound();
            }

            var catDoenca = await _context.TcatDoencas.FindAsync(id);
            if (catDoenca == null)
            {
                return NotFound();
            }
            return View(catDoenca);
        }

        // POST: CatDoencas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] CatDoenca catDoenca)
        {
            try
            {
                ViewBag.USER = UserLogado();
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home");
            }

            if (!UserLogado().IsAdmin)
            {
                return NotFound();
            }

            if (id != catDoenca.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(catDoenca);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CatDoencaExists(catDoenca.Id))
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
            return View(catDoenca);
        }

        // GET: CatDoencas/Delete/5
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

            if (!UserLogado().IsAdmin)
            {
                return NotFound();
            }

            if (id == null || _context.TcatDoencas == null)
            {
                return NotFound();
            }

            var catDoenca = await _context.TcatDoencas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (catDoenca == null)
            {
                return NotFound();
            }

            return View(catDoenca);
        }

        // POST: CatDoencas/Delete/5
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

            if (!UserLogado().IsAdmin)
            {
                return NotFound();
            }

            if (_context.TcatDoencas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TcatDoencas'  is null.");
            }
            var catDoenca = await _context.TcatDoencas.FindAsync(id);
            if (catDoenca != null)
            {
                _context.TcatDoencas.Remove(catDoenca);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CatDoencaExists(int id)
        {
          return _context.TcatDoencas.Any(e => e.Id == id);
        }
    }
}
