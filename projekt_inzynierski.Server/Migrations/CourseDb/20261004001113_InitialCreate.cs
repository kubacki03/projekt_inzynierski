using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace projekt_inzynierski.Server.Migrations.CourseDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeaturedCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturedCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeaturedCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCourses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Joined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EvaluationResult = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Description", "ImageURL", "IsPublic", "Language", "Level", "Title" },
                values: new object[,]
                {
                    { 1, "Podstawy programowania w języku C#: zmienne, pętle, klasy i obiekty.", "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056", true, "Polski", "Beginner", "C# dla początkujących" },
                    { 2, "Wprowadzenie do Pythona. Naucz się pisać skrypty, pracować z listami i funkcjami.", "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056", true, "Polski", "Beginner", "Python – pierwsze kroki" },
                    { 3, "Kurs skupiający się na fundamentach OOP w Javie: dziedziczenie, polimorfizm, interfejsy.", "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056", true, "Polski", "Intermediate", "Java – programowanie obiektowe" },
                    { 4, "Budowanie nowoczesnych API z użyciem ASP.NET Core i Entity Framework.", "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056", true, "Polski", "Intermediate", "ASP.NET Core – tworzenie API" },
                    { 5, "Naucz się JavaScript, DOM i podstaw Reacta, aby tworzyć dynamiczne aplikacje webowe.", "https://contentstatic.techgig.com/photo/114568844/10-weirdest-programming-languages-youve-never-heard-of.jpg?886056", true, "Polski", "Beginner", "JavaScript – od podstaw do React" }
                });

            migrationBuilder.InsertData(
                table: "FeaturedCourses",
                columns: new[] { "Id", "CourseId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Id",
                table: "Courses",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FeaturedCourses_CourseId",
                table: "FeaturedCourses",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_CourseId",
                table: "UserCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_UserId",
                table: "UserCourses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeaturedCourses");

            migrationBuilder.DropTable(
                name: "UserCourses");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
