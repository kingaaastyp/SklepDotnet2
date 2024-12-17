using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Uzytkownik
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Login jest wymagany")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        public string Haslo { get; set; }
        
        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(50, ErrorMessage = "Imię nie może przekraczać 50 znaków")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(50, ErrorMessage = "Nazwisko nie może przekraczać 50 znaków")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Adres jest wymagany")]
        [StringLength(200, ErrorMessage = "Adres nie może przekraczać 200 znaków")]
        public string Adres { get; set; }
    }
}