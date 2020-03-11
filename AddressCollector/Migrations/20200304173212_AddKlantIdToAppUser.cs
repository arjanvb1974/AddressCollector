using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressCollector.Migrations
{
    public partial class AddKlantIdToAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KlantId",
                table: "Address",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_KlantId",
                table: "Address",
                column: "KlantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_AspNetUsers_KlantId",
                table: "Address",
                column: "KlantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_AspNetUsers_KlantId",
                table: "Address");

            migrationBuilder.DropIndex(
                name: "IX_Address_KlantId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "KlantId",
                table: "Address");
        }
    }
}
