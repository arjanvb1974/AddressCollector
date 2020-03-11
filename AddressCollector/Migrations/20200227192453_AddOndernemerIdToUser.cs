using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressCollector.Migrations
{
    public partial class AddOndernemerIdToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OndernemerId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OndernemerId",
                table: "AspNetUsers");
        }
    }
}
