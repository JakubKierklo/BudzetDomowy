using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudzetDomowy.Server.Migrations
{
    /// <inheritdoc />
    public partial class PoczatkowaStruktura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kategorie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nazwa = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Typ = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transakcje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Kwota = table.Column<decimal>(type: "TEXT", precision: 12, scale: 2, nullable: false),
                    Opis = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    KategoriaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transakcje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transakcje_Kategorie_KategoriaId",
                        column: x => x.KategoriaId,
                        principalTable: "Kategorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Kategorie",
                columns: new[] { "Id", "Nazwa", "Typ" },
                values: new object[,]
                {
                    { 1, "Wynagrodzenie", "Przychod" },
                    { 2, "Inne przychody", "Przychod" },
                    { 3, "Jedzenie", "Wydatek" },
                    { 4, "Transport", "Wydatek" },
                    { 5, "Rachunki", "Wydatek" },
                    { 6, "Rozrywka", "Wydatek" },
                    { 7, "Zdrowie", "Wydatek" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transakcje_Data",
                table: "Transakcje",
                column: "Data");

            migrationBuilder.CreateIndex(
                name: "IX_Transakcje_KategoriaId",
                table: "Transakcje",
                column: "KategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transakcje");

            migrationBuilder.DropTable(
                name: "Kategorie");
        }
    }
}
