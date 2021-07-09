using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechShopSolution.Data.Migrations
{
    public partial class UpdateOrderAndTransportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isShip",
                table: "Order");

            migrationBuilder.AddColumn<int>(
                name: "ship_status",
                table: "Transport",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "status",
                table: "Order",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "cancel_at",
                table: "Order",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ship_status",
                table: "Transport");

            migrationBuilder.DropColumn(
                name: "cancel_at",
                table: "Order");

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "Order",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "isShip",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
