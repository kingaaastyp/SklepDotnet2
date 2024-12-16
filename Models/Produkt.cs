using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class Produkt
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa nie może przekraczać 100 znaków")]
        public string Nazwa { get; set; }

        [StringLength(500, ErrorMessage = "Opis nie może przekraczać 500 znaków")]
        public string Opis { get; set; }

        public string Zdjecie { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana")]
        public string Kategoria { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa niż 0")]
        public decimal Cena { get; set; }

        // Relacja z ZamowienieProdukt
        public ICollection<ZamowienieProdukt> ZamowienieProdukty { get; set; } = new List<ZamowienieProdukt>();
    }
}