﻿using System;
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
        public async Task<IActionResult> Index()
        {
            ViewBag.USER = UserLogado();

            var applicationDbContext = _context.Tutilizador.Include(u => u.Medico);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Utilizadores/Create
        public IActionResult Create()
        {
            ViewBag.USER = UserLogado();

            ViewData["MedicoId"] = new SelectList(_context.Tmedicos, "Id", "Nome");
            return View();
        }

        // POST: Utilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,User,Pass,MedicoId,IsAdmin")] Utilizador utilizador)
        {
            ViewBag.USER = UserLogado();

            if (ModelState.IsValid)
            {
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
            ViewBag.USER = UserLogado();

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,User,Pass,MedicoId,IsAdmin")] Utilizador utilizador)
        {
            ViewBag.USER = UserLogado();

            if (id != utilizador.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewBag.USER = UserLogado();

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
            ViewBag.USER = UserLogado();

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