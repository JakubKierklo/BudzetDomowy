using BudzetDomowy.Server.Data;
using BudzetDomowy.Server.Services;
using BudzetDomowy.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudzetDomowy.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PodsumowanieController(BudzetDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<PodsumowanieMiesiacaDto> Pobierz([FromQuery] int rok, [FromQuery] int miesiac)
    {
        var poczatek = new DateOnly(rok, miesiac, 1);
        var koniec = poczatek.AddMonths(1);

        var transakcje = await db.Transakcje
            .Include(t => t.Kategoria)
            .Where(t => t.Data >= poczatek && t.Data < koniec)
            .ToListAsync();

        return PodsumowanieObliczenia.Oblicz(rok, miesiac, transakcje);
    }
}
