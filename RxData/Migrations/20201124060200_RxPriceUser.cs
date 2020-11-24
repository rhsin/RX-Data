using Microsoft.EntityFrameworkCore.Migrations;

namespace RxData.Migrations
{
    public partial class RxPriceUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RxPriceUsers",
                columns: table => new
                {
                    RxPriceId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RxPriceUsers", x => new { x.RxPriceId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RxPriceUsers_RxPrices_RxPriceId",
                        column: x => x.RxPriceId,
                        principalTable: "RxPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RxPriceUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Ryan" });

            migrationBuilder.InsertData(
                table: "RxPriceUsers",
                columns: new[] { "RxPriceId", "UserId" },
                values: new object[] { 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_RxPriceUsers_UserId",
                table: "RxPriceUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RxPriceUsers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
