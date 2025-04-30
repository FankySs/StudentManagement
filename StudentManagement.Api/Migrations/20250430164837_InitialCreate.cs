using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagement.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rocniky",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cislo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rocniky", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tridy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kapacita = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tridy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uzivatele",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UzivatelskeJmeno = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HesloHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uzivatele", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predmety",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RocnikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmety", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Predmety_Rocniky_RocnikId",
                        column: x => x.RocnikId,
                        principalTable: "Rocniky",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Jmeno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prijmeni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumNarozeni = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TridaId = table.Column<int>(type: "int", nullable: false),
                    RocnikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Studenti_Rocniky_RocnikId",
                        column: x => x.RocnikId,
                        principalTable: "Rocniky",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Studenti_Tridy_TridaId",
                        column: x => x.TridaId,
                        principalTable: "Tridy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Znamky",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hodnota = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    PredmetId = table.Column<int>(type: "int", nullable: false),
                    UcitelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Znamky", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Znamky_Predmety_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmety",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Znamky_Studenti_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Studenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Znamky_Uzivatele_UcitelId",
                        column: x => x.UcitelId,
                        principalTable: "Uzivatele",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Predmety_RocnikId",
                table: "Predmety",
                column: "RocnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_RocnikId",
                table: "Studenti",
                column: "RocnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Studenti_TridaId",
                table: "Studenti",
                column: "TridaId");

            migrationBuilder.CreateIndex(
                name: "IX_Uzivatele_UzivatelskeJmeno",
                table: "Uzivatele",
                column: "UzivatelskeJmeno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Znamky_PredmetId",
                table: "Znamky",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Znamky_StudentId",
                table: "Znamky",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Znamky_UcitelId",
                table: "Znamky",
                column: "UcitelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Znamky");

            migrationBuilder.DropTable(
                name: "Predmety");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Uzivatele");

            migrationBuilder.DropTable(
                name: "Rocniky");

            migrationBuilder.DropTable(
                name: "Tridy");
        }
    }
}
