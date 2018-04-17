using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace School.Migrations
{
    public partial class orderchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Vmid",
                table: "Gx_vm_order",
                newName: "vmid");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Gx_vm_order",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Merchant_Name",
                table: "Gx_vm_order",
                newName: "merchant_name");

            migrationBuilder.RenameColumn(
                name: "Merchant_Id",
                table: "Gx_vm_order",
                newName: "merchant_id");

            migrationBuilder.RenameColumn(
                name: "Goods_Name",
                table: "Gx_vm_order",
                newName: "goods_name");

            migrationBuilder.RenameColumn(
                name: "Goods_Id",
                table: "Gx_vm_order",
                newName: "goods_id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Gx_vm_order",
                newName: "pay_price");

            migrationBuilder.RenameColumn(
                name: "PickupCode",
                table: "Gx_vm_order",
                newName: "transaction_id");

            migrationBuilder.RenameColumn(
                name: "PayTime",
                table: "Gx_vm_order",
                newName: "pay_time");

            migrationBuilder.RenameColumn(
                name: "PayChannel",
                table: "Gx_vm_order",
                newName: "pay_Channel");

            migrationBuilder.RenameColumn(
                name: "PayAccount",
                table: "Gx_vm_order",
                newName: "pickup_code");

            migrationBuilder.RenameColumn(
                name: "OrderNum",
                table: "Gx_vm_order",
                newName: "pay_account");

            migrationBuilder.RenameColumn(
                name: "DeliveryTime",
                table: "Gx_vm_order",
                newName: "delivery_time");

            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "Gx_vm_order",
                newName: "created_time");

            migrationBuilder.AddColumn<string>(
                name: "notify_url",
                table: "Gx_vm_order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "order_id",
                table: "Gx_vm_order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notify_url",
                table: "Gx_vm_order");

            migrationBuilder.DropColumn(
                name: "order_id",
                table: "Gx_vm_order");

            migrationBuilder.RenameColumn(
                name: "vmid",
                table: "Gx_vm_order",
                newName: "Vmid");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Gx_vm_order",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "merchant_name",
                table: "Gx_vm_order",
                newName: "Merchant_Name");

            migrationBuilder.RenameColumn(
                name: "merchant_id",
                table: "Gx_vm_order",
                newName: "Merchant_Id");

            migrationBuilder.RenameColumn(
                name: "goods_name",
                table: "Gx_vm_order",
                newName: "Goods_Name");

            migrationBuilder.RenameColumn(
                name: "goods_id",
                table: "Gx_vm_order",
                newName: "Goods_Id");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "Gx_vm_order",
                newName: "PickupCode");

            migrationBuilder.RenameColumn(
                name: "pickup_code",
                table: "Gx_vm_order",
                newName: "PayAccount");

            migrationBuilder.RenameColumn(
                name: "pay_time",
                table: "Gx_vm_order",
                newName: "PayTime");

            migrationBuilder.RenameColumn(
                name: "pay_price",
                table: "Gx_vm_order",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "pay_account",
                table: "Gx_vm_order",
                newName: "OrderNum");

            migrationBuilder.RenameColumn(
                name: "pay_Channel",
                table: "Gx_vm_order",
                newName: "PayChannel");

            migrationBuilder.RenameColumn(
                name: "delivery_time",
                table: "Gx_vm_order",
                newName: "DeliveryTime");

            migrationBuilder.RenameColumn(
                name: "created_time",
                table: "Gx_vm_order",
                newName: "CreatedTime");
        }
    }
}
