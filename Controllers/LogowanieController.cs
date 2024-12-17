using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Controllers
{
    public class LogowanieController : Controller
    {
        private readonly AppDbContext _context;

        public LogowanieController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Rejestracja
        public IActionResult Rejestracja()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Rejestracja(Uzytkownik uzytkownik)
        {
            if (ModelState.IsValid)
            {
                // Sprawdź, czy login jest już zajęty
                if (_context.Uzytkownicy.Any(u => u.Login == uzytkownik.Login))
                {
                    ModelState.AddModelError("", "Ten login jest już zajęty.");
                    return View(uzytkownik);
                }

                try
                {
                    // Hashowanie hasła
                    uzytkownik.Haslo = HashHaslo(uzytkownik.Haslo);
                    _context.Uzytkownicy.Add(uzytkownik);
                    _context.SaveChanges();

                    TempData["Message"] = "Rejestracja zakończona sukcesem! Możesz się teraz zalogować.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Błąd podczas zapisu do bazy: {ex.Message}");
                }
            }

            return View(uzytkownik);
        }


        // GET: Logowanie
        public IActionResult Login()
        {
            return View();
        }

        // POST: Logowanie
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string login, string password)
        {
            var user = _context.Uzytkownicy.FirstOrDefault(u => u.Login == login);

            if (user != null && WeryfikacjaHasla(password, user.Haslo))
            {
                HttpContext.Session.SetString("ZalogowanyLogin", user.Login);
                TempData["Message"] = "Zalogowano pomyślnie!";
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Nieprawidłowy login lub hasło.");
            return View();
        }

        public IActionResult Wyloguj()
        {
            HttpContext.Session.Clear(); 
            TempData["Message"] = "Wylogowano pomyślnie!";
            return RedirectToAction("Login");
        }
        private string HashHaslo(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        } 
        private bool WeryfikacjaHasla(string password, string hashedPassword)
        {
            return HashHaslo(password) == hashedPassword;
        }
    }
}
