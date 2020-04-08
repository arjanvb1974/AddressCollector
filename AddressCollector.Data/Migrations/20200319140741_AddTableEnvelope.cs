using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressCollector.Data.Migrations
{
    public partial class AddTableEnvelope : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Envelope",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Lengte = table.Column<int>(nullable: false),
                    Breedte = table.Column<int>(nullable: false),
                    OffsetLinks = table.Column<int>(nullable: false),
                    OffsetTop = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Envelope", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Envelope");
        }
    }
}
