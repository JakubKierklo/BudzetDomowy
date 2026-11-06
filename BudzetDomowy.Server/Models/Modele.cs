using BudzetDomowy.Shared;

namespace BudzetDomowy.Server.Models;

public class Kategoria
{
    public int Id { get; set; }
    public string Nazwa { get; set; } = "";
    public TypTransakcji Typ { get; set; }
}

public class Transakcja
{
    public int Id { get; set; }
    public DateOnly Data { get; set; }
    public decimal Kwota { get; set; }
    public string Opis { get; set; } = "";
    public int KategoriaId { get; set; }
    public Kategoria Kategoria { get; set; } = null!;
}
