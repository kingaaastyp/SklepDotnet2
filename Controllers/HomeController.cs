using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // Strona główna z listą produktów
        public async Task<IActionResult> Index()
        {
            var produkty = await _context.Produkty.ToListAsync();
            return View("Index", produkty); 
        }

        public IActionResult Privacy()
        {
            return View("Privacy"); 
        }

        // Widok zamówienia - wyświetla formularz z produktami
        public async Task<IActionResult> Zamow()
        {
            var produkty = await _context.Produkty.ToListAsync();
            ViewBag.Produkty = new SelectList(produkty, "Id", "Nazwa");
            return View(new Uzytkownik());
        }

        // Obsługa formularza zamówienia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Zamow(Uzytkownik uzytkownik, int[] wybraneProdukty)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Produkty = new SelectList(await _context.Produkty.ToListAsync(), "Id", "Nazwa");
                return View("Zamow", uzytkownik);
            }

            if (wybraneProdukty == null || wybraneProdukty.Length == 0)
            {
                ModelState.AddModelError("", "Musisz wybrać co najmniej jeden produkt.");
                ViewBag.Produkty = new SelectList(await _context.Produkty.ToListAsync(), "Id", "Nazwa");
                return View("Zamow", uzytkownik);
            }

            try
            {
                // Dodaj lub znajdź użytkownika
                var istniejącyUżytkownik = await _context.Uzytkownicy
                    .FirstOrDefaultAsync(u => u.Email == uzytkownik.Email);

                if (istniejącyUżytkownik == null)
                {
                    _context.Uzytkownicy.Add(uzytkownik);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    uzytkownik = istniejącyUżytkownik;
                }

                // Tworzenie zamówienia
                var zamowienie = new Zamowienie
                {
                    UzytkownikId = uzytkownik.Id,
                    DataZamowienia = DateTime.UtcNow
                };

                _context.Zamowienia.Add(zamowienie);
                await _context.SaveChangesAsync();

                // Powiązanie produktów z zamówieniem
                foreach (var produktId in wybraneProdukty)
                {
                    var zamowienieProdukt = new ZamowienieProdukt
                    {
                        ZamowienieId = zamowienie.Id,
                        ProduktId = produktId
                    };
                    _context.ZamowienieProdukty.Add(zamowienieProdukt);
                }

                await _context.SaveChangesAsync();

                // Przekierowanie do strony głównej
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.InnerException?.Message ?? ex.Message}");
                ModelState.AddModelError("", "Wystąpił błąd podczas zapisywania danych.");
                ViewBag.Produkty = new SelectList(await _context.Produkty.ToListAsync(), "Id", "Nazwa");
                return View("Zamow", uzytkownik);
            }
        }

        // Obsługa błędów
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
