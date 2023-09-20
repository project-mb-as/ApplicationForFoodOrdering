using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class swithcFromKorisnikToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Narudzba_Korisnik_KorisnikId",
                table: "Narudzba");

            migrationBuilder.DropIndex(
                name: "IX_Narudzba_KorisnikId",
                table: "Narudzba");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Narudzba");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeId",
                table: "User",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Narudzba",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_UserId",
                table: "Narudzba",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Narudzba_User_UserId",
                table: "Narudzba",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Narudzba_User_UserId",
                table: "Narudzba");

            migrationBuilder.DropIndex(
                name: "IX_Narudzba_UserId",
                table: "Narudzba");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TimeId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Narudzba");

            migrationBuilder.AddColumn<int>(
                name: "KorisnikId",
                table: "Narudzba",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_KorisnikId",
                table: "Narudzba",
                column: "KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Narudzba_Korisnik_KorisnikId",
                table: "Narudzba",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "KorisnikId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
