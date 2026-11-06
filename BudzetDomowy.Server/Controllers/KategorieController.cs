using BudzetDomowy.Server.Data;
using BudzetDomowy.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudzetDomowy.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KategorieController(BudzetDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<List<KategoriaDto>> Pobierz() =>
        await db.Kategorie
            .OrderBy(k => k.Typ)
            .ThenBy(k => k.Nazwa)
            .Select(k => new KategoriaDto(k.Id, k.Nazwa, k.Typ))
            .ToListAsync();
}
