using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProduktyController : Controller
    {
        private readonly AppDbContext _context;

        public ProduktyController(AppDbContext context)
        {
            _context = context;
        }

        private async Task<IEnumerable<string>> PobierzKategorie()
        {
            return await _context.Produkty
                .Select(p => p.Kategoria)
                .Distinct()
                .ToListAsync();
        }

        // GET: Produkty/WedlugKategorii
        public async Task<IActionResult> WedlugKategorii(string kategoria)
        {
            ViewBag.Kategorie = await PobierzKategorie();

            var produkty = string.IsNullOrEmpty(kategoria) ?
                await _context.Produkty.ToListAsync() :
                await _context.Produkty.Where(p => p.Kategoria == kategoria).ToListAsync();

            ViewBag.WybranaKategoria = kategoria;

            return View("WedlugKategorii", produkty); // Szuka w Views/Home
        }

        // GET: Produkty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var produkt = await _context.Produkty.FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null) return NotFound();

            return View("Details", produkt); // Szuka w Views/Home
        }

        // GET: Produkty/Create
        public IActionResult Create()
        {
            return View("Create"); // Szuka w Views/Home
        }

        // POST: Produkty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produkt produkt)
        {
            if (!ModelState.IsValid) return View("Create", produkt);

            try
            {
                _context.Produkty.Add(produkt);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
                return View("Create", produkt);
            }
        }

        // GET: Produkty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt == null) return NotFound();

            return View("Edit", produkt); // Szuka w Views/Home
        }

        // POST: Produkty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Opis,Zdjecie,Kategoria,Cena")] Produkt produkt)
        {
            if (id != produkt.Id) return NotFound();

            if (!ModelState.IsValid) return View("Edit", produkt);

            try
            {
                _context.Update(produkt);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Produkty.Any(e => e.Id == id)) return NotFound();
                throw;
            }
        }

        // GET: Produkty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var produkt = await _context.Produkty.FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null) return NotFound();

            return View("Delete", produkt); // Szuka w Views/Home
        }

        // POST: Produkty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produkt = await _context.Produkty.FindAsync(id);
            if (produkt != null)
            {
                _context.Produkty.Remove(produkt);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
