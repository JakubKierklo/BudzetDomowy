using BudzetDomowy.Server.Models;
using BudzetDomowy.Shared;

namespace BudzetDomowy.Server.Services;

// Czysta logika bez bazy danych – dzięki temu da się ją łatwo testować.
public static class PodsumowanieObliczenia
{
    public static PodsumowanieMiesiacaDto Oblicz(int rok, int miesiac, IEnumerable<Transakcja> transakcje)
    {
        var lista = transakcje.ToList();

        var przychody = lista
            .Where(t => t.Kategoria.Typ == TypTransakcji.Przychod)
            .Sum(t => t.Kwota);

        var wydatki = lista
            .Where(t => t.Kategoria.Typ == TypTransakcji.Wydatek)
            .Sum(t => t.Kwota);

        var wydatkiWgKategorii = lista
            .Where(t => t.Kategoria.Typ == TypTransakcji.Wydatek)
            .GroupBy(t => t.Kategoria.Nazwa)
            .Select(g => new SumaKategoriiDto(g.Key, g.Sum(t => t.Kwota)))
            .OrderByDescending(s => s.Suma)
            .ToList();

        return new PodsumowanieMiesiacaDto(
            rok,
            miesiac,
            przychody,
            wydatki,
            przychody - wydatki,
            wydatkiWgKategorii);
    }
}
