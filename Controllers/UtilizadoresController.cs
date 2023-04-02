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
    public class UtilizadoresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }
        public Utilizador UserLogado()
        {
            int? idUSer = HttpContext.Session.GetInt32("idUser");
            var utilizador = _context.Tutilizador.Include(u => u.Medico).Single(u => u.Id == idUSer);
            return utilizador;
        }

        // GET: Utilizadores
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
                var filtro = _context.Tutilizador
                    .Include(m => m.Medico)
                    .Where(m => m.User.ToLower().Contains(pesquisa.ToLower()) || m.Medico.Nome.ToLower().Contains(pesquisa.ToLower()));
                return View(await filtro.ToListAsync());

            }

            var applicationDbContext = _context.Tutilizador.Include(u => u.Medico);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Utilizadores/Create
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

            ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Nome");
            return View();
        }

        // POST: Utilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,User,Pass,MedicoId,IsAdmin")] Utilizador utilizador, IFormFile imagem, string user)
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
            var userRepetido = _context.Tutilizador
                .SingleOrDefault(u=>u.User == utilizador.User);

            if (userRepetido != null)
            {
                ViewBag.USERREPETIDO = "Este user já está a ser usado tente outro!";

                ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Nome", utilizador.MedicoId);
                return View(utilizador);
            }

            if (ModelState.IsValid && imagem != null && imagem.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/users", user + ".png");

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imagem.CopyToAsync(stream);
                }

                _context.Add(utilizador);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Nome", utilizador.MedicoId);
            return View(utilizador);
        }

        // GET: Utilizadores/Edit/5
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

            if (id == null || _context.Tutilizador == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Tutilizador.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }
            ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Nome", utilizador.MedicoId);
            return View(utilizador);
        }

        // POST: Utilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,User,Pass,MedicoId,IsAdmin")] Utilizador utilizador, IFormFile imagem, string user)
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

            if (id != utilizador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && imagem != null && imagem.Length > 0)
            {
                try
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/users", user + ".png");

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imagem.CopyToAsync(stream);
                    }

                    _context.Update(utilizador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadorExists(utilizador.Id))
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
            ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Nome", utilizador.MedicoId);
            return View(utilizador);
        }

        // GET: Utilizadores/Delete/5
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

            if (id == null || _context.Tutilizador == null)
            {
                return NotFound();
            }

            var utilizador = await _context.Tutilizador
                .Include(u => u.Medico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (utilizador == null)
            {
                return NotFound();
            }

            return View(utilizador);
        }

        // POST: Utilizadores/Delete/5
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

            if (_context.Tutilizador == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tutilizador'  is null.");
            }
            var utilizador = await _context.Tutilizador.FindAsync(id);
            if (utilizador != null)
            {
                _context.Tutilizador.Remove(utilizador);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadorExists(int id)
        {
          return (_context.Tutilizador?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
