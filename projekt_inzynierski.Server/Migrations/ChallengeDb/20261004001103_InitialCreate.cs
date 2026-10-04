using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace projekt_inzynierski.Server.Migrations.ChallengeDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BadgeId = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBadges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BadgeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBadges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBadges_Badges_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Badges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChallenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    ChallengeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChallenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChallenges_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyChallenges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChallengeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyChallenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyChallenges_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "Id", "ImagePath", "Title" },
                values: new object[,]
                {
                    { 1, "klasy_i_obiekty_badge.png", "Mistrz klas" },
                    { 2, "pierwsze_kroki.png", "Pierwsze kroki" },
                    { 3, "for.webp", "Mistrz pętli" },
                    { 4, "pierwsze_kroki_csharp.webp", "Pierwsze kroki C#" },
                    { 5, "listy.webp", "Listy" },
                    { 6, "linq.webp", "Linq" },
                    { 7, "meotdy.jpg", "Metody" },
                    { 8, "dziedziczenie.jpg", "Dziedziczenie" },
                    { 9, "interfejsy.jpg", "Interfejsy" },
                    { 10, "pliki.png", "Pliki" },
                    { 11, "wyjatki.png", "Wyjatki" },
                    { 12, "async.png", "Asynchronicznosc" },
                    { 13, "bazy_danych.png", "Bazy danych" },
                    { 14, "test.jpg", "Testy jednostkowe" },
                    { 15, "rest_api.jpg", "Api Rest" },
                    { 16, "dependency.webp", "Dependency Injection" },
                    { 17, "events.png", "Delegaty i zdarzenia" },
                    { 18, "generyki.png", "Generyki" }
                });

            migrationBuilder.InsertData(
                table: "Challenges",
                columns: new[] { "Id", "BadgeId", "Description", "Name", "Points", "RewardDescription", "Type" },
                values: new object[,]
                {
                    { 1, 0, "Napisz prostą aplikację Hello World.", "Pierwsze kroki w C#", 50, "Odznaka: Początkujący", "Coding" },
                    { 2, 3, "Zaimplementuj program wypisujący liczby od 1 do 100.", "Pętla for", 70, "Odznaka: Iteracje", "Coding" },
                    { 3, 4, "Sprawdź, czy liczba jest parzysta lub nieparzysta.", "Warunki", 80, "Odznaka: Logika", "Coding" },
                    { 4, 5, "Stwórz listę i wypisz jej elementy.", "Listy", 100, "Odznaka: Kolekcje", "Coding" },
                    { 5, 7, "Zaimplementuj metodę obliczającą silnię.", "Metody", 120, "Odznaka: Rekurencja", "Coding" },
                    { 6, 1, "Stwórz klasę reprezentującą samochód.", "Klasy i obiekty", 150, "Odznaka: OOP", "Coding" },
                    { 7, 8, "Zaimplementuj klasę bazową i klasę dziedziczącą.", "Dziedziczenie", 200, "Odznaka: OOP+", "Coding" },
                    { 8, 9, "Stwórz interfejs i zaimplementuj go w klasie.", "Interfejsy", 250, "Odznaka: Architekt", "Coding" },
                    { 9, 10, "Napisz program zapisujący i odczytujący tekst z pliku.", "Pliki", 180, "Odznaka: IO", "Coding" },
                    { 10, 6, "Użyj LINQ do wyszukania liczb parzystych w kolekcji.", "LINQ podstawy", 220, "Odznaka: LINQ", "Coding" },
                    { 11, 11, "Napisz kod, który przechwyci i obsłuży wyjątek dzielenia przez zero.", "Obsługa wyjątków", 160, "Odznaka: TryCatch", "Coding" },
                    { 12, 12, "Zaimplementuj metodę asynchroniczną korzystającą z async/await.", "Asynchroniczność", 300, "Odznaka: Async", "Coding" },
                    { 13, 13, "Połącz się z bazą danych i odczytaj listę rekordów.", "Baza danych", 350, "Odznaka: DataAccess", "Coding" },
                    { 14, 14, "Napisz test jednostkowy dla prostej metody kalkulatora.", "Testy jednostkowe", 200, "Odznaka: Tester", "Coding" },
                    { 15, 15, "Stwórz prosty kontroler API zwracający listę obiektów JSON.", "API REST", 400, "Odznaka: WebDev", "Coding" },
                    { 16, 16, "Skonfiguruj wstrzykiwanie zależności w aplikacji ASP.NET Core.", "Dependency Injection", 450, "Odznaka: Architektura", "Coding" },
                    { 17, 17, "Utwórz delegata i zdarzenie, a następnie je wywołaj.", "Delegaty i zdarzenia", 280, "Odznaka: EventMaster", "Coding" },
                    { 18, 18, "Zaimplementuj klasę generyczną działającą dla różnych typów danych.", "Generics", 320, "Odznaka: Generics", "Coding" },
                    { 19, 2, "Napisz prostą aplikację Hello World w dowolnym języku.", "Pierwsze kroki", 10, "Odznaka: Początkujący", "Coding" }
                });

            migrationBuilder.InsertData(
                table: "WeeklyChallenges",
                columns: new[] { "Id", "ChallengeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBadges_BadgeId",
                table: "UserBadges",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChallenges_ChallengeId",
                table: "UserChallenges",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyChallenges_ChallengeId",
                table: "WeeklyChallenges",
                column: "ChallengeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBadges");

            migrationBuilder.DropTable(
                name: "UserChallenges");

            migrationBuilder.DropTable(
                name: "WeeklyChallenges");

            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "Challenges");
        }
    }
}
