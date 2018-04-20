using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace School.Migrations
{
    public partial class Iwadawdaw : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "s_operator_tree",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "AbpUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "s_operator_tree");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "AbpUsers");
        }
    }
}
