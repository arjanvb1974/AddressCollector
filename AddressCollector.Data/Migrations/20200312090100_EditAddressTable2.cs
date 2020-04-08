using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressCollector.Data.Migrations
{
    public partial class EditAddressTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Address_LandId",
                table: "Address",
                column: "LandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Country_LandId",
                table: "Address",
                column: "LandId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Country_LandId",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_LandId",
                table: "Address");
        }
    }
}
