using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace School.Migrations
{
    public partial class bussinegss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "s_operator_tree",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    TreeCode = table.Column<string>(nullable: true),
                    TreeLength = table.Column<int>(nullable: false),
                    TreeName = table.Column<string>(maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_operator_tree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_s_operator_tree_s_operator_tree_ParentId",
                        column: x => x.ParentId,
                        principalTable: "s_operator_tree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "s_point",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    Latitide = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    PointAddress = table.Column<string>(nullable: true),
                    PointDescription = table.Column<string>(nullable: true),
                    PointName = table.Column<string>(maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_point", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "s_device",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    DeviceName = table.Column<string>(nullable: true),
                    DeviceNum = table.Column<string>(maxLength: 128, nullable: false),
                    DeviceType = table.Column<string>(nullable: true),
                    PointId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_s_device_s_point_PointId",
                        column: x => x.PointId,
                        principalTable: "s_point",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "s_operator_device",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    DeviceId = table.Column<int>(nullable: false),
                    IsSeal = table.Column<bool>(nullable: false),
                    OperatorId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_s_operator_device", x => x.Id);
                    table.ForeignKey(
                        name: "FK_s_operator_device_s_device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "s_device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_s_operator_device_s_operator_tree_OperatorId",
                        column: x => x.OperatorId,
                        principalTable: "s_operator_tree",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_s_device_PointId",
                table: "s_device",
                column: "PointId");

            migrationBuilder.CreateIndex(
                name: "IX_s_operator_device_DeviceId",
                table: "s_operator_device",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_s_operator_device_OperatorId",
                table: "s_operator_device",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_s_operator_tree_ParentId",
                table: "s_operator_tree",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "s_operator_device");

            migrationBuilder.DropTable(
                name: "s_device");

            migrationBuilder.DropTable(
                name: "s_operator_tree");

            migrationBuilder.DropTable(
                name: "s_point");
        }
    }
}
