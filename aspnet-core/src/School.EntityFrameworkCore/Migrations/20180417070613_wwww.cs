using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace School.Migrations
{
    public partial class wwww : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "s_point",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "s_device_goods",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "s_device",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "s_point");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "s_device_goods");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "s_device");
        }
    }
}
