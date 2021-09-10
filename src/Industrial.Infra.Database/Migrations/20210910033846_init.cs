using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace Industrial.Infra.Database.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KG_Item",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KG_Item", x => x.ItemID);
                });

            migrationBuilder.CreateTable(
                name: "KG_Job",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    JobUniqueID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KG_Job", x => x.JobId);
                });

            migrationBuilder.CreateTable(
                name: "KG_Location",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LoadStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KG_Location", x => x.LocationID);
                });

            migrationBuilder.CreateTable(
                name: "KG_Now",
                columns: table => new
                {
                    ContainerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LocationID = table.Column<int>(type: "int", nullable: false),
                    EnterTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KG_Now", x => x.ContainerID);
                    table.ForeignKey(
                        name: "FK_KG_Now_KG_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "KG_Location",
                        principalColumn: "LocationID",
                        onUpdate: ReferentialAction.SetNull,
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KG_NowMes",
                columns: table => new
                {
                    ContainerID = table.Column<int>(type: "int", nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    ItemID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KG_NowMes", x => x.ContainerID);
                    table.ForeignKey(
                        name: "FK_KG_NowMes_KG_Item_ItemID",
                        column: x => x.ItemID,
                        principalTable: "KG_Item",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KG_NowMes_KG_Now_ContainerID",
                        column: x => x.ContainerID,
                        principalTable: "KG_Now",
                        principalColumn: "ContainerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KG_Now_LocationID",
                table: "KG_Now",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_KG_NowMes_ItemID",
                table: "KG_NowMes",
                column: "ItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KG_Job");

            migrationBuilder.DropTable(
                name: "KG_NowMes");

            migrationBuilder.DropTable(
                name: "KG_Item");

            migrationBuilder.DropTable(
                name: "KG_Now");

            migrationBuilder.DropTable(
                name: "KG_Location");
        }
    }
}
