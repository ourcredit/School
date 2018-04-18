using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace School.Migrations
{
    public partial class adwadawd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillConnection",
                table: "s_device",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BillStatus",
                table: "s_device",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoinConnection",
                table: "s_device",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cointype0Lack",
                table: "s_device",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cointype1Lack",
                table: "s_device",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoorSw",
                table: "s_device",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SaleStatus",
                table: "s_device",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkPattern",
                table: "s_device",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillConnection",
                table: "s_device");

            migrationBuilder.DropColumn(
                name: "BillStatus",
                table: "s_device");

            migrationBuilder.DropColumn(
                name: "CoinConnection",
                table: "s_device");

            migrationBuilder.DropColumn(
                name: "Cointype0Lack",
                table: "s_device");

            migrationBuilder.DropColumn(
                name: "Cointype1Lack",
                table: "s_device");

            migrationBuilder.DropColumn(
                name: "DoorSw",
                table: "s_device");

            migrationBuilder.DropColumn(
                name: "SaleStatus",
                table: "s_device");

            migrationBuilder.DropColumn(
                name: "WorkPattern",
                table: "s_device");
        }
    }
}
