﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApplication1.Data;

#nullable disable

namespace WebApplication1.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241217163343_LOGiHAS2")]
    partial class LOGiHAS2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebApplication1.Models.Produkt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Cena")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<string>("Kategoria")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Opis")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Zdjecie")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Produkty");
                });

            modelBuilder.Entity("WebApplication1.Models.Uzytkownik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Adres")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Haslo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Imie")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nazwisko")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("Uzytkownicy");
                });

            modelBuilder.Entity("WebApplication1.Models.Zamowienie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DataZamowienia")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UzytkownikId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UzytkownikId");

                    b.ToTable("Zamowienia");
                });

            modelBuilder.Entity("WebApplication1.Models.ZamowienieProdukt", b =>
                {
                    b.Property<int>("ZamowienieId")
                        .HasColumnType("integer");

                    b.Property<int>("ProduktId")
                        .HasColumnType("integer");

                    b.HasKey("ZamowienieId", "ProduktId");

                    b.HasIndex("ProduktId");

                    b.ToTable("ZamowienieProdukty");
                });

            modelBuilder.Entity("WebApplication1.Models.Zamowienie", b =>
                {
                    b.HasOne("WebApplication1.Models.Uzytkownik", "Uzytkownik")
                        .WithMany()
                        .HasForeignKey("UzytkownikId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Uzytkownik");
                });

            modelBuilder.Entity("WebApplication1.Models.ZamowienieProdukt", b =>
                {
                    b.HasOne("WebApplication1.Models.Produkt", "Produkt")
                        .WithMany("ZamowienieProdukty")
                        .HasForeignKey("ProduktId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApplication1.Models.Zamowienie", "Zamowienie")
                        .WithMany("ZamowienieProdukty")
                        .HasForeignKey("ZamowienieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Produkt");

                    b.Navigation("Zamowienie");
                });

            modelBuilder.Entity("WebApplication1.Models.Produkt", b =>
                {
                    b.Navigation("ZamowienieProdukty");
                });

            modelBuilder.Entity("WebApplication1.Models.Zamowienie", b =>
                {
                    b.Navigation("ZamowienieProdukty");
                });
#pragma warning restore 612, 618
        }
    }
}
