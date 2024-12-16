using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class Zamowienie
    {
        public int Id { get; set; }
        public DateTime DataZamowienia { get; set; }
        public int UzytkownikId { get; set; }
        public Uzytkownik Uzytkownik { get; set; }

        // Nawigacja do tabeli pośredniej
        public ICollection<ZamowienieProdukt> ZamowienieProdukty { get; set; } = new List<ZamowienieProdukt>();
    }
}