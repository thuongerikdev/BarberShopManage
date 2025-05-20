using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BM.WebAPI.Migrations.BookingDb
{
    /// <inheritdoc />
    public partial class BookingV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingProductDetails_BookingSuppliers_supplierID",
                table: "BookingProductDetails");

            migrationBuilder.DropIndex(
                name: "IX_BookingProductDetails_supplierID",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "productDescriptionID",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "supplierID",
                table: "BookingProductDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "createAt",
                table: "BookingProductDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "productColor",
                table: "BookingProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "productDescription",
                table: "BookingProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "productImage",
                table: "BookingProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "productName",
                table: "BookingProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "productNote",
                table: "BookingProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "productSize",
                table: "BookingProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "productStatus",
                table: "BookingProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "productType",
                table: "BookingProductDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "updateAt",
                table: "BookingProductDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "BookingSupplierProductDetails",
                columns: table => new
                {
                    supplierProductDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productDetailID = table.Column<int>(type: "int", nullable: false),
                    supplierID = table.Column<int>(type: "int", nullable: false),
                    productPrice = table.Column<double>(type: "float", nullable: false),
                    productQuantity = table.Column<int>(type: "int", nullable: false),
                    productStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingSupplierProductDetails", x => x.supplierProductDetailID);
                    table.ForeignKey(
                        name: "FK_BookingSupplierProductDetails_BookingProductDetails_supplierID",
                        column: x => x.supplierID,
                        principalTable: "BookingProductDetails",
                        principalColumn: "productDetailID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingSupplierProductDetails_BookingSuppliers_supplierID",
                        column: x => x.supplierID,
                        principalTable: "BookingSuppliers",
                        principalColumn: "supplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingSupplierProductDetails_supplierID",
                table: "BookingSupplierProductDetails",
                column: "supplierID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingSupplierProductDetails");

            migrationBuilder.DropColumn(
                name: "createAt",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "productColor",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "productDescription",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "productImage",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "productName",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "productNote",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "productSize",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "productStatus",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "productType",
                table: "BookingProductDetails");

            migrationBuilder.DropColumn(
                name: "updateAt",
                table: "BookingProductDetails");

            migrationBuilder.AddColumn<int>(
                name: "productDescriptionID",
                table: "BookingProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "supplierID",
                table: "BookingProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookingProductDetails_supplierID",
                table: "BookingProductDetails",
                column: "supplierID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingProductDetails_BookingSuppliers_supplierID",
                table: "BookingProductDetails",
                column: "supplierID",
                principalTable: "BookingSuppliers",
                principalColumn: "supplierID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
