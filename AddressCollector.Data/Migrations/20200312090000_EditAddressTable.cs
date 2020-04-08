using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressCollector.Data.Migrations
{
    public partial class EditAddressTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Land",
                table: "Address");

            migrationBuilder.AddColumn<int>(
                name: "LandId",
                table: "Address",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LandId",
                table: "Address");

            migrationBuilder.AddColumn<string>(
                name: "Land",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
