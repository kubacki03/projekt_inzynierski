using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace projekt_inzynierski.Server.Migrations.AchievmentDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    RuleKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RuleConfigJson = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "{}"),
                    BadgeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillCategory = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudyDays",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Day = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyDays", x => new { x.UserId, x.Day });
                });

            migrationBuilder.CreateTable(
                name: "UserProgress",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProgress", x => new { x.UserId, x.LessonId });
                });

            migrationBuilder.CreateTable(
                name: "UserAchievements",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AchievementId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EarnedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchievements", x => new { x.UserId, x.AchievementId });
                    table.ForeignKey(
                        name: "FK_UserAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Achievements",
                columns: new[] { "Id", "BadgeType", "Description", "Name", "Points", "RuleConfigJson", "RuleKey", "SkillCategory" },
                values: new object[,]
                {
                    { "5-lessons", "silver", "Ukończ 5 lekcji", "Rozkręcamy się", 25, "{\"Threshold\":5}", "LessonCount", "common" },
                    { "7-day-streak", "silver", "Ucz się 7 dni z rzędu", "Szczwany Streak", 40, "{\"Days\":7}", "Streak", "common" },
                    { "first-lesson", "bronze", "Ukończ pierwszą lekcję", "Pierwsza lekcja!", 10, "{\"Threshold\":1}", "LessonCount", "common" },
                    { "polyglot", "gold", "Ukończ lekcje w 2 różnych językach", "Poliglota kodu", 30, "{\"DistinctLanguages\":2}", "LanguageExplorer", "common" },
                    { "quiz-90", "silver", "Zdobądź minimum 90% w quizie.", "Mistrz quizów", 100, "{\"MinScore\":90}", "quiz-score", "common" },
                    { "task-1", "bronze", "Ukończ swoje pierwsze zadanie.", "Pierwsze zadanie!", 10, "{\"Threshold\":1}", "task-count", "common" },
                    { "task-10", "gold", "Ukończ 10 zadań.", "10 zadań zaliczonych", 50, "{\"Threshold\":10}", "task-count", "common" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_AchievementId",
                table: "UserAchievements",
                column: "AchievementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudyDays");

            migrationBuilder.DropTable(
                name: "UserAchievements");

            migrationBuilder.DropTable(
                name: "UserProgress");

            migrationBuilder.DropTable(
                name: "Achievements");
        }
    }
}
