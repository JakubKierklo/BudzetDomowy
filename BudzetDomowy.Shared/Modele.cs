using System.ComponentModel.DataAnnotations;

namespace BudzetDomowy.Shared;

public enum TypTransakcji
{
    Przychod = 1,
    Wydatek = 2
}

public record KategoriaDto(int Id, string Nazwa, TypTransakcji Typ);

public record TransakcjaDto(
    int Id,
    DateOnly Data,
    decimal Kwota,
    string Opis,
    int KategoriaId,
    string NazwaKategorii,
    TypTransakcji Typ);

// Klasa (a nie record), żeby Blazor mógł bindować ją bezpośrednio do formularza.
public class TransakcjaZapisDto
{
    [Required(ErrorMessage = "Podaj datę.")]
    public DateOnly Data { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Range(0.01, 1000000, ErrorMessage = "Kwota musi być większa od zera.")]
    public decimal Kwota { get; set; }

    [Required(ErrorMessage = "Podaj opis.")]
    [MaxLength(200, ErrorMessage = "Opis może mieć maksymalnie 200 znaków.")]
    public string Opis { get; set; } = "";

    [Range(1, int.MaxValue, ErrorMessage = "Wybierz kategorię.")]
    public int KategoriaId { get; set; }
}

public record SumaKategoriiDto(string Nazwa, decimal Suma);

public record PodsumowanieMiesiacaDto(
    int Rok,
    int Miesiac,
    decimal Przychody,
    decimal Wydatki,
    decimal Saldo,
    IReadOnlyList<SumaKategoriiDto> WydatkiWgKategorii);
