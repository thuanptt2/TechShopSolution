using Microsoft.EntityFrameworkCore.Migrations;

namespace TechShopSolution.Data.Migrations
{
    public partial class UpdateTableCoupon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "max_price",
                table: "Coupon",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "min_order_value",
                table: "Coupon",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "max_price",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "min_order_value",
                table: "Coupon");
        }
    }
}
