using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class updateOcjena : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ocjena_Korisnik_KorisnikId",
                table: "Ocjena");

            migrationBuilder.DropIndex(
                name: "IX_Ocjena_KorisnikId",
                table: "Ocjena");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Ocjena");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ocjena",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ocjena_UserId",
                table: "Ocjena",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ocjena_User_UserId",
                table: "Ocjena",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ocjena_User_UserId",
                table: "Ocjena");

            migrationBuilder.DropIndex(
                name: "IX_Ocjena_UserId",
                table: "Ocjena");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ocjena");

            migrationBuilder.AddColumn<int>(
                name: "KorisnikId",
                table: "Ocjena",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ocjena_KorisnikId",
                table: "Ocjena",
                column: "KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ocjena_Korisnik_KorisnikId",
                table: "Ocjena",
                column: "KorisnikId",
                principalTable: "Korisnik",
                principalColumn: "KorisnikId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
