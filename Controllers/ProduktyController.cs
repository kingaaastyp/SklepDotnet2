using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> WedlugKategorii(string kategoria)
        {
            ViewBag.Kategorie = await PobierzKategorie();

            var produkty = string.IsNullOrEmpty(kategoria) ?
                await _context.Produkty.ToListAsync() :
                await _context.Produkty.Where(p => p.Kategoria == kategoria).ToListAsync();

            ViewBag.WybranaKategoria = kategoria;

            return View("WedlugKategorii", produkty); 
        }
        
        public IActionResult Create()
        {
            return View("Create"); 
        }

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

     
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var produkt = await _context.Produkty.FirstOrDefaultAsync(m => m.Id == id);
            if (produkt == null) return NotFound();

            return View("Delete", produkt);
        }
        
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
