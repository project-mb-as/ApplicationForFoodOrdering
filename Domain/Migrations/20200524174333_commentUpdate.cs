using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class commentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Komentar_Korisnik_KorisnikId",
                table: "Komentar");

            migrationBuilder.DropIndex(
                name: "IX_Komentar_KorisnikId",
                table: "Komentar");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Komentar");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Komentar",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Hrana",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Komentar_UserId",
                table: "Komentar",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Komentar_User_UserId",
                table: "Komentar",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Komentar_User_UserId",
                table: "Komentar");

            migrationBuilder.DropIndex(
                name: "IX_Komentar_UserId",
                table: "Komentar");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Komentar");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Hrana");

            migrationBuilder.AddColumn<int>(
                name: "KorisnikId",
                table: "Komentar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Komentar_KorisnikId",
                table: "Komentar",
                column: "KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Komentar_Korisnik_KorisnikId",
                table: "Komentar",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "KorisnikId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
