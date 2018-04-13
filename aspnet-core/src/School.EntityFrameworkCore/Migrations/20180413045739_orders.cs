using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace School.Migrations
{
    public partial class orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gx_vm_channel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Goods_Id = table.Column<int>(nullable: false),
                    Isdelete = table.Column<bool>(nullable: false),
                    Machine_Code = table.Column<string>(nullable: true),
                    Quantity = table.Column<float>(nullable: false),
                    QuantityLine = table.Column<float>(nullable: false),
                    Site = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gx_vm_channel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gx_vm_order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    DeliveryTime = table.Column<DateTime>(nullable: true),
                    Goods_Id = table.Column<int>(nullable: false),
                    Goods_Name = table.Column<string>(nullable: true),
                    Merchant_Id = table.Column<int>(nullable: false),
                    Merchant_Name = table.Column<string>(nullable: true),
                    PayAccount = table.Column<string>(nullable: true),
                    PayChannel = table.Column<int>(nullable: false),
                    PayTime = table.Column<DateTime>(nullable: true),
                    PickupCode = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Value = table.Column<float>(nullable: false),
                    Vmid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gx_vm_order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gx_vm_Show",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChannelSite = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Isdelete = table.Column<bool>(nullable: false),
                    Machine_Code = table.Column<string>(nullable: true),
                    QuantityLine = table.Column<float>(nullable: false),
                    ShowSite = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gx_vm_Show", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gx_vm_channel");

            migrationBuilder.DropTable(
                name: "Gx_vm_order");

            migrationBuilder.DropTable(
                name: "Gx_vm_Show");
        }
    }
}
