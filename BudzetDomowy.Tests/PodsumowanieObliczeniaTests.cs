using BudzetDomowy.Server.Models;
using BudzetDomowy.Server.Services;
using BudzetDomowy.Shared;
using Xunit;

namespace BudzetDomowy.Tests;

public class PodsumowanieObliczeniaTests
{
    private static readonly Kategoria Pensja = new() { Id = 1, Nazwa = "Wynagrodzenie", Typ = TypTransakcji.Przychod };
    private static readonly Kategoria Jedzenie = new() { Id = 3, Nazwa = "Jedzenie", Typ = TypTransakcji.Wydatek };
    private static readonly Kategoria Transport = new() { Id = 4, Nazwa = "Transport", Typ = TypTransakcji.Wydatek };

    private static Transakcja Tx(Kategoria kategoria, decimal kwota) => new()
    {
        Data = new DateOnly(2026, 3, 10),
        Kwota = kwota,
        Opis = "test",
        KategoriaId = kategoria.Id,
        Kategoria = kategoria
    };

    [Fact]
    public void Oblicz_SumujePrzychodyWydatkiISaldo()
    {
        var transakcje = new[]
        {
            Tx(Pensja, 5000m),
            Tx(Jedzenie, 1200.50m),
            Tx(Transport, 300m)
        };

        var wynik = PodsumowanieObliczenia.Oblicz(2026, 3, transakcje);

        Assert.Equal(5000m, wynik.Przychody);
        Assert.Equal(1500.50m, wynik.Wydatki);
        Assert.Equal(3499.50m, wynik.Saldo);
    }

    [Fact]
    public void Oblicz_GrupujeWydatkiWgKategoriiMalejaco()
    {
        var transakcje = new[]
        {
            Tx(Transport, 100m),
            Tx(Jedzenie, 250m),
            Tx(Jedzenie, 50m),
            Tx(Pensja, 9999m) // przychody nie trafiają do wydatków wg kategorii
        };

        var wynik = PodsumowanieObliczenia.Oblicz(2026, 3, transakcje);

        Assert.Collection(wynik.WydatkiWgKategorii,
            pierwsza =>
            {
                Assert.Equal("Jedzenie", pierwsza.Nazwa);
                Assert.Equal(300m, pierwsza.Suma);
            },
            druga =>
            {
                Assert.Equal("Transport", druga.Nazwa);
                Assert.Equal(100m, druga.Suma);
            });
    }

    [Fact]
    public void Oblicz_BezTransakcji_ZwracaZera()
    {
        var wynik = PodsumowanieObliczenia.Oblicz(2026, 3, Array.Empty<Transakcja>());

        Assert.Equal(0m, wynik.Przychody);
        Assert.Equal(0m, wynik.Wydatki);
        Assert.Equal(0m, wynik.Saldo);
        Assert.Empty(wynik.WydatkiWgKategorii);
    }

    [Fact]
    public void Oblicz_SaldoUjemne_GdyWydatkiPrzekraczajaPrzychody()
    {
        var transakcje = new[]
        {
            Tx(Pensja, 1000m),
            Tx(Jedzenie, 1500m)
        };

        var wynik = PodsumowanieObliczenia.Oblicz(2026, 3, transakcje);

        Assert.Equal(-500m, wynik.Saldo);
    }
}
