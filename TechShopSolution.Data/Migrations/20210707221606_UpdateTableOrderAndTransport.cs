using Microsoft.EntityFrameworkCore.Migrations;

namespace TechShopSolution.Data.Migrations
{
    public partial class UpdateTableOrderAndTransport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "from_address",
                table: "Transport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "to_address",
                table: "Transport",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cancel_reason",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "from_address",
                table: "Transport");

            migrationBuilder.DropColumn(
                name: "to_address",
                table: "Transport");

            migrationBuilder.DropColumn(
                name: "cancel_reason",
                table: "Order");
        }
    }
}
