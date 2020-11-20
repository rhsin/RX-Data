using Microsoft.EntityFrameworkCore.Migrations;

namespace RxData.Migrations
{
    public partial class RxPriceFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Dose",
                table: "RxPrices",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "RxPrices",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "RxPrices",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dose",
                table: "RxPrices");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "RxPrices");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "RxPrices");
        }
    }
}
