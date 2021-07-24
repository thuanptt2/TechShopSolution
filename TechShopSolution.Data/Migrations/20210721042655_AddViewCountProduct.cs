using Microsoft.EntityFrameworkCore.Migrations;

namespace TechShopSolution.Data.Migrations
{
    public partial class AddViewCountProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "view_count",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "view_count",
                table: "Product");
        }
    }
}
