using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace projekt_inzynierski.Server.Migrations.StoreDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvatarRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvatarId = table.Column<int>(type: "int", nullable: false),
                    RewardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvatarRewards_Avatars_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Avatars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAvatars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarId = table.Column<int>(type: "int", nullable: false),
                    UnlockedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAvatars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAvatars_Avatars_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Avatars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRewards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RewardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRewards_Rewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Rewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Avatars",
                columns: new[] { "Id", "ImageUrl", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Images/blade-runner.png", true, "Łowca androidów" },
                    { 2, "Images/clockwork-orange.jpg", true, "Nakręcony człowiek" },
                    { 3, "Images/hal-9000.png", true, "Nieomylne AI" },
                    { 4, "Images/interstellar.jpg", true, "Podróż w nieznane" },
                    { 5, "Images/moon.webp", true, "Nie jesteś sam" },
                    { 6, "Images/tehc.jpg", true, "Tech" },
                    { 7, "Images/trip.webp", true, "Memodestruktor" },
                    { 8, "Images/default-avatar.jpg", true, "Memodestruktor" },
                    { 9, "Images/water.webp", true, "Wodna planeta" },
                    { 10, "Images/wide.webp", true, "Oczy zamkniete" },
                    { 11, "Images/space.jpg", true, "W nadprzestrzen" },
                    { 12, "Images/turnhal.jpg", true, "Wylaczenie" },
                    { 13, "Images/truman.webp", true, "Prawda" },
                    { 14, "Images/vendetta.jpg", true, "Prawda" }
                });

            migrationBuilder.InsertData(
                table: "Rewards",
                columns: new[] { "Id", "Cost", "Description", "ImageUrl", "IsActive", "Name", "Type" },
                values: new object[,]
                {
                    { 1, 150, "fajny obrazek", "Images/blade-runner.png", true, "Awatar", "Awatar" },
                    { 2, 100, "fajny obrazek", "Images/clockwork-orange.jpg", true, "Awatar", "Awatar" },
                    { 3, 100, "fajny obrazek", "Images/hal-9000.png", true, "Awatar", "Awatar" },
                    { 4, 110, "fajny obrazek", "Images/interstellar.jpg", true, "Awatar", "Awatar" },
                    { 5, 200, "fajny obrazek", "Images/moon.webp", true, "Awatar", "Awatar" },
                    { 6, 50, "fajny obrazek", "Images/tehc.jpg", true, "Awatar", "Awatar" },
                    { 7, 100, "fajny obrazek", "Images/trip.webp", true, "Awatar", "Awatar" },
                    { 8, 150, "fajny obrazek", "Images/water.webp", true, "Awatar", "Awatar" },
                    { 9, 250, "fajny obrazek", "Images/wide.webp", true, "Awatar", "Awatar" },
                    { 10, 150, "fajny obrazek", "Images/space2001.jpg", true, "Awatar", "Awatar" },
                    { 11, 350, "fajny obrazek", "Images/turnhal.jpg", true, "Awatar", "Awatar" },
                    { 12, 450, "fajny obrazek", "Images/truman.webp", true, "Awatar", "Awatar" },
                    { 13, 100, "fajny obrazek", "Images/vendetta.jpg", true, "Awatar", "Awatar" },
                    { 14, 500, "Zyskaj dodatkowe 100% PD", "Images/newPremium1.png", true, "Konto premium na dzień", "Premium1" },
                    { 15, 500, "Zyskaj dodatkowe 100% PD", "Images/newPremium7.png", true, "Konto premium na tydzień", "Premium7" },
                    { 16, 500, "Zyskaj dodatkowe 100% PD", "Images/newPremium30.png", true, "Konto premium na miesiąc", "Premium30" }
                });

            migrationBuilder.InsertData(
                table: "AvatarRewards",
                columns: new[] { "Id", "AvatarId", "RewardId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 },
                    { 3, 3, 3 },
                    { 4, 4, 4 },
                    { 5, 5, 5 },
                    { 6, 6, 6 },
                    { 7, 7, 7 },
                    { 8, 9, 8 },
                    { 9, 10, 9 },
                    { 10, 11, 10 },
                    { 11, 12, 11 },
                    { 12, 13, 12 },
                    { 13, 14, 13 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvatarRewards_AvatarId",
                table: "AvatarRewards",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAvatars_AvatarId",
                table: "UserAvatars",
                column: "AvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_RewardId",
                table: "UserRewards",
                column: "RewardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvatarRewards");

            migrationBuilder.DropTable(
                name: "UserAvatars");

            migrationBuilder.DropTable(
                name: "UserRewards");

            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropTable(
                name: "Rewards");
        }
    }
}
