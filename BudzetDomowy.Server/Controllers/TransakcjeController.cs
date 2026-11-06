using BudzetDomowy.Server.Data;
using BudzetDomowy.Server.Models;
using BudzetDomowy.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudzetDomowy.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransakcjeController(BudzetDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TransakcjaDto>>> Pobierz([FromQuery] int rok, [FromQuery] int miesiac)
    {
        var poczatek = new DateOnly(rok, miesiac, 1);
        var koniec = poczatek.AddMonths(1);

        var transakcje = await db.Transakcje
            .Include(t => t.Kategoria)
            .Where(t => t.Data >= poczatek && t.Data < koniec)
            .OrderByDescending(t => t.Data)
            .ToListAsync();

        return transakcje.Select(Mapuj).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<TransakcjaDto>> Dodaj(TransakcjaZapisDto dane)
    {
        var kategoria = await db.Kategorie.FindAsync(dane.KategoriaId);
        if (kategoria is null)
        {
            return BadRequest("Wybrana kategoria nie istnieje.");
        }

        var transakcja = new Transakcja
        {
            Data = dane.Data,
            Kwota = dane.Kwota,
            Opis = dane.Opis,
            KategoriaId = dane.KategoriaId,
            Kategoria = kategoria
        };

        db.Transakcje.Add(transakcja);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(Pobierz), new { rok = transakcja.Data.Year, miesiac = transakcja.Data.Month }, Mapuj(transakcja));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Usun(int id)
    {
        var transakcja = await db.Transakcje.FindAsync(id);
        if (transakcja is null)
        {
            return NotFound();
        }

        db.Transakcje.Remove(transakcja);
        await db.SaveChangesAsync();

        return NoContent();
    }

    private static TransakcjaDto Mapuj(Transakcja t) =>
        new(t.Id, t.Data, t.Kwota, t.Opis, t.KategoriaId, t.Kategoria.Nazwa, t.Kategoria.Typ);
}
