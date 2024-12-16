using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class ZamowienieProdukt
    {
        public int ZamowienieId { get; set; }
        public Zamowienie Zamowienie { get; set; }

        public int ProduktId { get; set; }
        public Produkt Produkt { get; set; }

    }
}