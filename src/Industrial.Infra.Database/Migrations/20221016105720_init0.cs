using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Industrial.Infra.Database.Migrations
{
    public partial class init0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "latin1");

            migrationBuilder.CreateTable(
                name: "kg_item",
                columns: table => new
                {
                    ItemID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "text", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.ItemID);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "kg_job",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    JobUniqueID = table.Column<long>(type: "bigint(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.JobId);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "kg_location",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(type: "varchar(7)", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1"),
                    LoadStatus = table.Column<string>(type: "varchar(4)", nullable: false, collation: "latin1_swedish_ci")
                        .Annotation("MySql:CharSet", "latin1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.LocationID);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "kg_now",
                columns: table => new
                {
                    ContainerID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LocationID = table.Column<int>(type: "int(11)", nullable: false),
                    EnterTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.ContainerID);
                    table.ForeignKey(
                        name: "FK_KG_Now_KG_Location_LocationID",
                        column: x => x.LocationID,
                        principalTable: "kg_location",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateTable(
                name: "kg_nowmes",
                columns: table => new
                {
                    ContainerID = table.Column<int>(type: "int(11)", nullable: false),
                    Qty = table.Column<int>(type: "int(11)", nullable: false),
                    ItemID = table.Column<int>(type: "int(11)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.ContainerID);
                    table.ForeignKey(
                        name: "FK_KG_NowMes_KG_Item_ItemID",
                        column: x => x.ItemID,
                        principalTable: "kg_item",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KG_NowMes_KG_Now_ContainerID",
                        column: x => x.ContainerID,
                        principalTable: "kg_now",
                        principalColumn: "ContainerID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "latin1")
                .Annotation("Relational:Collation", "latin1_swedish_ci");

            migrationBuilder.CreateIndex(
                name: "IX_KG_Now_LocationID",
                table: "kg_now",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_KG_NowMes_ItemID",
                table: "kg_nowmes",
                column: "ItemID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kg_job");

            migrationBuilder.DropTable(
                name: "kg_nowmes");

            migrationBuilder.DropTable(
                name: "kg_item");

            migrationBuilder.DropTable(
                name: "kg_now");

            migrationBuilder.DropTable(
                name: "kg_location");
        }
    }
}
