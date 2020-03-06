using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressCollector.Migrations
{
    public partial class ChangeColumnTypePostalCodesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "EndRange",
                table: "PostalCode",
                type: "numeric(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BeginRange",
                table: "PostalCode",
                type: "numeric(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "EndRange",
                table: "PostalCode",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18, 0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BeginRange",
                table: "PostalCode",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18, 0)");
        }
    }
}
