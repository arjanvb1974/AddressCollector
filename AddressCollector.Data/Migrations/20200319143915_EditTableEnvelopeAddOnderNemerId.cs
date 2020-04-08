using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressCollector.Data.Migrations
{
    public partial class EditTableEnvelopeAddOnderNemerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OnderNemerId",
                table: "Envelope",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Envelope_OnderNemerId",
                table: "Envelope",
                column: "OnderNemerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Envelope_AspNetUsers_OnderNemerId",
                table: "Envelope",
                column: "OnderNemerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Envelope_AspNetUsers_OnderNemerId",
                table: "Envelope");

            migrationBuilder.DropIndex(
                name: "IX_Envelope_OnderNemerId",
                table: "Envelope");

            migrationBuilder.DropColumn(
                name: "OnderNemerId",
                table: "Envelope");
        }
    }
}
