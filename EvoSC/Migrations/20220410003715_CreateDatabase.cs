using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EvoSC.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nickname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UbisoftName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Group = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Banned = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LastVisit = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Uid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FilePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Enabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManiaExchangeId = table.Column<int>(type: "int", nullable: false),
                    ManiaExchangeVersion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maps_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Player_Statistics",
                columns: table => new
                {
                    PlayerStatisticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Visits = table.Column<int>(type: "int", nullable: false),
                    PlayTime = table.Column<int>(type: "int", nullable: false),
                    Finishes = table.Column<int>(type: "int", nullable: false),
                    LocalRecords = table.Column<int>(type: "int", nullable: false),
                    Ratings = table.Column<int>(type: "int", nullable: false),
                    Wins = table.Column<int>(type: "int", nullable: false),
                    Donations = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    SpectatorTime = table.Column<int>(type: "int", nullable: false),
                    CheckpointsDriven = table.Column<int>(type: "int", nullable: false),
                    ConsecutiveDaysPlayed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player_Statistics", x => x.PlayerStatisticId);
                    table.ForeignKey(
                        name: "FK_Player_Statistics_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Map_Karma",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    New = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MapId = table.Column<int>(type: "int", nullable: true),
                    PlayerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map_Karma", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Map_Karma_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Map_Karma_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Map_Records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    Checkpoints = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    MapId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map_Records", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Map_Records_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Map_Records_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Map_Statistics",
                columns: table => new
                {
                    MapStatisticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MapId = table.Column<int>(type: "int", nullable: false),
                    NumberOfPlays = table.Column<int>(type: "int", nullable: false),
                    Cooldown = table.Column<int>(type: "int", nullable: false),
                    LastPlayed = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AmountSkipped = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Map_Statistics", x => x.MapStatisticId);
                    table.ForeignKey(
                        name: "FK_Map_Statistics_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Player_MapFavorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    MapId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player_MapFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_MapFavorites_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Player_MapFavorites_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Player_PersonalBests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Checkpoints = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: true),
                    MapId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player_PersonalBests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_PersonalBests_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Player_PersonalBests_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Map_Karma_MapId",
                table: "Map_Karma",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Map_Karma_PlayerId",
                table: "Map_Karma",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Map_Records_MapId",
                table: "Map_Records",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Map_Records_PlayerId",
                table: "Map_Records",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Map_Statistics_MapId",
                table: "Map_Statistics",
                column: "MapId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maps_PlayerId",
                table: "Maps",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_MapFavorites_MapId",
                table: "Player_MapFavorites",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_MapFavorites_PlayerId",
                table: "Player_MapFavorites",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_PersonalBests_MapId",
                table: "Player_PersonalBests",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_PersonalBests_PlayerId",
                table: "Player_PersonalBests",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Statistics_PlayerId",
                table: "Player_Statistics",
                column: "PlayerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Map_Karma");

            migrationBuilder.DropTable(
                name: "Map_Records");

            migrationBuilder.DropTable(
                name: "Map_Statistics");

            migrationBuilder.DropTable(
                name: "Player_MapFavorites");

            migrationBuilder.DropTable(
                name: "Player_PersonalBests");

            migrationBuilder.DropTable(
                name: "Player_Statistics");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
