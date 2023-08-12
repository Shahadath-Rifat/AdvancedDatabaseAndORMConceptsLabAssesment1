using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class Initial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "locations",
                columns: table => new
                {
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_locations", x => x.StoreID);
                });

            migrationBuilder.CreateTable(
                name: "laptops",
                columns: table => new
                {
                    LaptopID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_laptops", x => x.LaptopID);
                    table.ForeignKey(
                        name: "FK_laptops_brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "brands",
                        principalColumn: "BrandId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LaptopStores",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreAddressStoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LaptopID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaptopStores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LaptopStores_laptops_LaptopID",
                        column: x => x.LaptopID,
                        principalTable: "laptops",
                        principalColumn: "LaptopID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LaptopStores_locations_StoreAddressStoreID",
                        column: x => x.StoreAddressStoreID,
                        principalTable: "locations",
                        principalColumn: "StoreID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_laptops_BrandId",
                table: "laptops",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_LaptopStores_LaptopID",
                table: "LaptopStores",
                column: "LaptopID");

            migrationBuilder.CreateIndex(
                name: "IX_LaptopStores_StoreAddressStoreID",
                table: "LaptopStores",
                column: "StoreAddressStoreID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LaptopStores");

            migrationBuilder.DropTable(
                name: "laptops");

            migrationBuilder.DropTable(
                name: "locations");

            migrationBuilder.DropTable(
                name: "brands");
        }
    }
}
