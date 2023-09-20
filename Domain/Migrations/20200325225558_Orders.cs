using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class Orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hrana",
                columns: table => new
                {
                    HranaId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Stalna = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hrana", x => x.HranaId);
                });

            migrationBuilder.CreateTable(
                name: "Korisnik",
                columns: table => new
                {
                    KorisnikId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(nullable: false),
                    Prezime = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Lozinka = table.Column<string>(nullable: true),
                    LocationId = table.Column<int>(nullable: false),
                    TimeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnik", x => x.KorisnikId);
                });

            migrationBuilder.CreateTable(
                name: "Meni",
                columns: table => new
                {
                    MeniId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Datum = table.Column<DateTime>(type: "Date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meni", x => x.MeniId);
                });

            migrationBuilder.CreateTable(
                name: "Prilog",
                columns: table => new
                {
                    PrilogId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prilog", x => x.PrilogId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Roles = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Komentar",
                columns: table => new
                {
                    KomentarId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HranaId = table.Column<int>(nullable: false),
                    Slika = table.Column<string>(nullable: true),
                    KorisnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Komentar", x => x.KomentarId);
                    table.ForeignKey(
                        name: "FK_Komentar_Hrana_HranaId",
                        column: x => x.HranaId,
                        principalTable: "Hrana",
                        principalColumn: "HranaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Komentar_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ocjena",
                columns: table => new
                {
                    OcjenaId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Vrijednost = table.Column<int>(nullable: false),
                    HranaId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocjena", x => x.OcjenaId);
                    table.ForeignKey(
                        name: "FK_Ocjena_Hrana_HranaId",
                        column: x => x.HranaId,
                        principalTable: "Hrana",
                        principalColumn: "HranaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ocjena_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HranaMeni",
                columns: table => new
                {
                    HranaId = table.Column<int>(nullable: false),
                    MeniId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HranaMeni", x => new { x.HranaId, x.MeniId });
                    table.ForeignKey(
                        name: "FK_HranaMeni_Hrana_HranaId",
                        column: x => x.HranaId,
                        principalTable: "Hrana",
                        principalColumn: "HranaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HranaMeni_Meni_MeniId",
                        column: x => x.MeniId,
                        principalTable: "Meni",
                        principalColumn: "MeniId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Narudzba",
                columns: table => new
                {
                    NarudzbaId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MeniId = table.Column<int>(nullable: false),
                    HranaId = table.Column<int>(nullable: false),
                    KorisnikId = table.Column<int>(nullable: false),
                    Dostavljena = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    LocationId = table.Column<int>(nullable: false),
                    TimeId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Narudzba", x => x.NarudzbaId);
                    table.ForeignKey(
                        name: "FK_Narudzba_Korisnik_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnik",
                        principalColumn: "KorisnikId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Narudzba_Meni_MeniId",
                        column: x => x.MeniId,
                        principalTable: "Meni",
                        principalColumn: "MeniId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HranaPrilog",
                columns: table => new
                {
                    HranaId = table.Column<int>(nullable: false),
                    PrilogId = table.Column<int>(nullable: false),
                    HranaPrilogId = table.Column<int>(nullable: false),
                    Varijanta = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HranaPrilog", x => new { x.HranaId, x.PrilogId });
                    table.ForeignKey(
                        name: "FK_HranaPrilog_Hrana_HranaId",
                        column: x => x.HranaId,
                        principalTable: "Hrana",
                        principalColumn: "HranaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HranaPrilog_Prilog_PrilogId",
                        column: x => x.PrilogId,
                        principalTable: "Prilog",
                        principalColumn: "PrilogId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderSideDish",
                columns: table => new
                {
                    NarudzbaId = table.Column<int>(nullable: false),
                    PrilogId = table.Column<int>(nullable: false),
                    OrderSideDishId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSideDish", x => new { x.NarudzbaId, x.PrilogId });
                    table.ForeignKey(
                        name: "FK_OrderSideDish_Narudzba_NarudzbaId",
                        column: x => x.NarudzbaId,
                        principalTable: "Narudzba",
                        principalColumn: "NarudzbaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderSideDish_Prilog_PrilogId",
                        column: x => x.PrilogId,
                        principalTable: "Prilog",
                        principalColumn: "PrilogId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HranaMeni_MeniId",
                table: "HranaMeni",
                column: "MeniId");

            migrationBuilder.CreateIndex(
                name: "IX_HranaPrilog_PrilogId",
                table: "HranaPrilog",
                column: "PrilogId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentar_HranaId",
                table: "Komentar",
                column: "HranaId");

            migrationBuilder.CreateIndex(
                name: "IX_Komentar_KorisnikId",
                table: "Komentar",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Meni_Datum",
                table: "Meni",
                column: "Datum",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_KorisnikId",
                table: "Narudzba",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_MeniId",
                table: "Narudzba",
                column: "MeniId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocjena_HranaId",
                table: "Ocjena",
                column: "HranaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ocjena_KorisnikId",
                table: "Ocjena",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSideDish_PrilogId",
                table: "OrderSideDish",
                column: "PrilogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HranaMeni");

            migrationBuilder.DropTable(
                name: "HranaPrilog");

            migrationBuilder.DropTable(
                name: "Komentar");

            migrationBuilder.DropTable(
                name: "Ocjena");

            migrationBuilder.DropTable(
                name: "OrderSideDish");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Hrana");

            migrationBuilder.DropTable(
                name: "Narudzba");

            migrationBuilder.DropTable(
                name: "Prilog");

            migrationBuilder.DropTable(
                name: "Korisnik");

            migrationBuilder.DropTable(
                name: "Meni");
        }
    }
}
