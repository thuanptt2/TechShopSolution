using Microsoft.EntityFrameworkCore.Migrations;

namespace TechShopSolution.Data.Migrations
{
    public partial class UpdateTableOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "subtotal",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "coupon_id",
                table: "Order",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_coupon_id",
                table: "Order",
                column: "coupon_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Coupon_coupon_id",
                table: "Order",
                column: "coupon_id",
                principalTable: "Coupon",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Coupon_coupon_id",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_coupon_id",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "coupon_id",
                table: "Order");

            migrationBuilder.AddColumn<decimal>(
                name: "subtotal",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
