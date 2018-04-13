using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace School.Migrations
{
    public partial class chage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "s_operator_device",
                nullable: false,
                oldClrType: typeof(Guid))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AbpUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TreeCode",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OperatorDeviceGoods",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    GoodsId = table.Column<int>(nullable: false),
                    GoodsName = table.Column<string>(nullable: true),
                    OperatorDeviceId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorDeviceGoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperatorDeviceGoods_s_operator_device_OperatorDeviceId",
                        column: x => x.OperatorDeviceId,
                        principalTable: "s_operator_device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperatorDeviceGoods_OperatorDeviceId",
                table: "OperatorDeviceGoods",
                column: "OperatorDeviceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperatorDeviceGoods");

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "TreeCode",
                table: "AbpUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "s_operator_device",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}
