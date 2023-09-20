using Microsoft.EntityFrameworkCore.Migrations;

namespace Domain.Migrations
{
    public partial class orderFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Narudzba_HranaId",
                table: "Narudzba",
                column: "HranaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Narudzba_Hrana_HranaId",
                table: "Narudzba",
                column: "HranaId",
                principalTable: "Hrana",
                principalColumn: "HranaId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Narudzba_Hrana_HranaId",
                table: "Narudzba");

            migrationBuilder.DropIndex(
                name: "IX_Narudzba_HranaId",
                table: "Narudzba");
        }
    }
}
