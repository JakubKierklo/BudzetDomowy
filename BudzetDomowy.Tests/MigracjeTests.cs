using BudzetDomowy.Server.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BudzetDomowy.Tests;

public class MigracjeTests
{
    [Fact]
    public void Migrate_NaPustejBazie_TworzyTabeleIDodajeKategorie()
    {
        // Baza w pamięci istnieje tylko dopóki połączenie jest otwarte.
        using var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<BudzetDbContext>()
            .UseSqlite(connection)
            .Options;

        using var db = new BudzetDbContext(options);
        db.Database.Migrate();

        Assert.NotEmpty(db.Kategorie);
        Assert.Empty(db.Transakcje);
    }
}
