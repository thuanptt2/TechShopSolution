using Microsoft.EntityFrameworkCore.Migrations;

namespace TechShopSolution.Data.Migrations
{
    public partial class ChangeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Coupon_coupon_id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_cus_id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_PaymentMethod_payment_id",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_order_id",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_product_id",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Transport_Order_order_id",
                table: "Transport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "OrderDetail",
                newName: "BillDetail");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Bill");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_product_id",
                table: "BillDetail",
                newName: "IX_BillDetail_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_Order_payment_id",
                table: "Bill",
                newName: "IX_Bill_payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_Order_cus_id",
                table: "Bill",
                newName: "IX_Bill_cus_id");

            migrationBuilder.RenameIndex(
                name: "IX_Order_coupon_id",
                table: "Bill",
                newName: "IX_Bill_coupon_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BillDetail",
                table: "BillDetail",
                columns: new[] { "order_id", "product_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bill",
                table: "Bill",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Coupon_coupon_id",
                table: "Bill",
                column: "coupon_id",
                principalTable: "Coupon",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_Customer_cus_id",
                table: "Bill",
                column: "cus_id",
                principalTable: "Customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_PaymentMethod_payment_id",
                table: "Bill",
                column: "payment_id",
                principalTable: "PaymentMethod",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetail_Bill_order_id",
                table: "BillDetail",
                column: "order_id",
                principalTable: "Bill",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetail_Product_product_id",
                table: "BillDetail",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transport_Bill_order_id",
                table: "Transport",
                column: "order_id",
                principalTable: "Bill",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Coupon_coupon_id",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Bill_Customer_cus_id",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_Bill_PaymentMethod_payment_id",
                table: "Bill");

            migrationBuilder.DropForeignKey(
                name: "FK_BillDetail_Bill_order_id",
                table: "BillDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_BillDetail_Product_product_id",
                table: "BillDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Transport_Bill_order_id",
                table: "Transport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BillDetail",
                table: "BillDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bill",
                table: "Bill");

            migrationBuilder.RenameTable(
                name: "BillDetail",
                newName: "OrderDetail");

            migrationBuilder.RenameTable(
                name: "Bill",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_BillDetail_product_id",
                table: "OrderDetail",
                newName: "IX_OrderDetail_product_id");

            migrationBuilder.RenameIndex(
                name: "IX_Bill_payment_id",
                table: "Order",
                newName: "IX_Order_payment_id");

            migrationBuilder.RenameIndex(
                name: "IX_Bill_cus_id",
                table: "Order",
                newName: "IX_Order_cus_id");

            migrationBuilder.RenameIndex(
                name: "IX_Bill_coupon_id",
                table: "Order",
                newName: "IX_Order_coupon_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail",
                columns: new[] { "order_id", "product_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Coupon_coupon_id",
                table: "Order",
                column: "coupon_id",
                principalTable: "Coupon",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_cus_id",
                table: "Order",
                column: "cus_id",
                principalTable: "Customer",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_PaymentMethod_payment_id",
                table: "Order",
                column: "payment_id",
                principalTable: "PaymentMethod",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_order_id",
                table: "OrderDetail",
                column: "order_id",
                principalTable: "Order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_product_id",
                table: "OrderDetail",
                column: "product_id",
                principalTable: "Product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transport_Order_order_id",
                table: "Transport",
                column: "order_id",
                principalTable: "Order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
