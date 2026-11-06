using BudzetDomowy.Server.Models;
using BudzetDomowy.Shared;
using Microsoft.EntityFrameworkCore;

namespace BudzetDomowy.Server.Data;

public class BudzetDbContext(DbContextOptions<BudzetDbContext> options) : DbContext(options)
{
    public DbSet<Kategoria> Kategorie => Set<Kategoria>();
    public DbSet<Transakcja> Transakcje => Set<Transakcja>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategoria>(e =>
        {
            e.Property(k => k.Nazwa).HasMaxLength(100).IsRequired();
            e.Property(k => k.Typ).HasConversion<string>();

            // Dane startowe – trafiają do bazy razem z migracją.
            e.HasData(
                new Kategoria { Id = 1, Nazwa = "Wynagrodzenie", Typ = TypTransakcji.Przychod },
                new Kategoria { Id = 2, Nazwa = "Inne przychody", Typ = TypTransakcji.Przychod },
                new Kategoria { Id = 3, Nazwa = "Jedzenie", Typ = TypTransakcji.Wydatek },
                new Kategoria { Id = 4, Nazwa = "Transport", Typ = TypTransakcji.Wydatek },
                new Kategoria { Id = 5, Nazwa = "Rachunki", Typ = TypTransakcji.Wydatek },
                new Kategoria { Id = 6, Nazwa = "Rozrywka", Typ = TypTransakcji.Wydatek },
                new Kategoria { Id = 7, Nazwa = "Zdrowie", Typ = TypTransakcji.Wydatek });
        });

        modelBuilder.Entity<Transakcja>(e =>
        {
            e.Property(t => t.Kwota).HasPrecision(12, 2);
            e.Property(t => t.Opis).HasMaxLength(200).IsRequired();
            e.HasIndex(t => t.Data);
            e.HasOne(t => t.Kategoria)
                .WithMany()
                .HasForeignKey(t => t.KategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
