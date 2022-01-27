using Microsoft.EntityFrameworkCore.Migrations;

namespace Hendry_Mason_HW5.Migrations
{
    public partial class productsuppliers_setup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSupplier");

            migrationBuilder.CreateTable(
                name: "ProductSuppliers",
                columns: table => new
                {
                    ProductSupplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: true),
                    SupplierID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSuppliers", x => x.ProductSupplierID);
                    table.ForeignKey(
                        name: "FK_ProductSuppliers_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSuppliers_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSuppliers_ProductID",
                table: "ProductSuppliers",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSuppliers_SupplierID",
                table: "ProductSuppliers",
                column: "SupplierID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSuppliers");

            migrationBuilder.CreateTable(
                name: "ProductSupplier",
                columns: table => new
                {
                    ProductsProductID = table.Column<int>(type: "int", nullable: false),
                    SuppliersSupplierID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSupplier", x => new { x.ProductsProductID, x.SuppliersSupplierID });
                    table.ForeignKey(
                        name: "FK_ProductSupplier_Products_ProductsProductID",
                        column: x => x.ProductsProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSupplier_Suppliers_SuppliersSupplierID",
                        column: x => x.SuppliersSupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSupplier_SuppliersSupplierID",
                table: "ProductSupplier",
                column: "SuppliersSupplierID");
        }
    }
}
