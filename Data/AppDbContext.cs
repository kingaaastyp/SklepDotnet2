using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Produkt> Produkty { get; set; }
        public DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public DbSet<Zamowienie> Zamowienia { get; set; }
        public DbSet<ZamowienieProdukt> ZamowienieProdukty { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguracja klucza złożonego dla ZP
            modelBuilder.Entity<ZamowienieProdukt>()
                .HasKey(zp => new { zp.ZamowienieId, zp.ProduktId });

            modelBuilder.Entity<ZamowienieProdukt>()
                .HasOne(zp => zp.Zamowienie)
                .WithMany(z => z.ZamowienieProdukty)
                .HasForeignKey(zp => zp.ZamowienieId);

            modelBuilder.Entity<ZamowienieProdukt>()
                .HasOne(zp => zp.Produkt)
                .WithMany(p => p.ZamowienieProdukty)
                .HasForeignKey(zp => zp.ProduktId);

            modelBuilder.Entity<Produkt>()
                .Property(p => p.Cena)
                .HasPrecision(18, 2);
        }
    }
}