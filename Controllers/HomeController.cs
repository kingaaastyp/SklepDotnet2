using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var produkty = await _context.Produkty.ToListAsync();
            return View("Index", produkty);
        }

        public async Task<IActionResult> Zamow()
        {
            var sessionLogin = HttpContext.Session.GetString("ZalogowanyLogin");

            if (string.IsNullOrEmpty(sessionLogin))
            {
                TempData["Message"] = "Aby złożyć zamówienie, musisz się zalogować.";
                return RedirectToAction("Login", "Logowanie");
            }

            var user = await _context.Uzytkownicy.FirstOrDefaultAsync(u => u.Login == sessionLogin);

            if (user == null)
            {
                TempData["Message"] = "Nie znaleziono zalogowanego użytkownika.";
                return RedirectToAction("Login", "Logowanie");
            }

            var model = new Uzytkownik
            {
                Adres = user.Adres,
                Imie = user.Imie,
                Nazwisko = user.Nazwisko,
                Email = user.Email,
                Login = user.Login
            };

            var produkty = await _context.Produkty.ToListAsync();
            ViewBag.Produkty = produkty;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Zamow(int[] wybraneProdukty)
        {
            var sessionLogin = HttpContext.Session.GetString("ZalogowanyLogin");
            if (string.IsNullOrEmpty(sessionLogin))
            {
                TempData["Message"] = "Musisz się zalogować, aby złożyć zamówienie.";
                return RedirectToAction("Login", "Logowanie");
            }

            if (wybraneProdukty == null || wybraneProdukty.Length == 0)
            {
                ModelState.AddModelError("", "Musisz wybrać co najmniej jeden produkt.");
                ViewBag.Produkty = await _context.Produkty.ToListAsync();
                return View();
            }

            try
            {
                var user = await _context.Uzytkownicy.FirstOrDefaultAsync(u => u.Login == sessionLogin);
                if (user == null)
                {
                    TempData["Message"] = "Nie znaleziono zalogowanego użytkownika.";
                    return RedirectToAction("Login", "Logowanie");
                }
                
                var zamowienie = new Zamowienie
                {
                    UzytkownikId = user.Id,
                    DataZamowienia = DateTime.UtcNow
                };

                _context.Zamowienia.Add(zamowienie);
                await _context.SaveChangesAsync();
                
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
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
                ModelState.AddModelError("", "Wystąpił błąd podczas składania zamówienia.");
                ViewBag.Produkty = await _context.Produkty.ToListAsync();
                return View();
            }
        }
    }
}