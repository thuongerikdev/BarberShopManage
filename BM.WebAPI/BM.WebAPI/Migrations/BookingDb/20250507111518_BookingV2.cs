using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BM.WebAPI.Migrations.BookingDb
{
    /// <inheritdoc />
    public partial class BookingV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingProductDetails_BookingProductDescriptions_productDescriptionID",
                table: "BookingProductDetails");

            migrationBuilder.DropTable(
                name: "BookingProductDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_BookingProductDetails_productDescriptionID",
                table: "BookingProductDetails");

            migrationBuilder.CreateTable(
                name: "BookingProductImages",
                columns: table => new
                {
                    productImageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    srcImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingProductImages", x => x.productImageID);
                    table.ForeignKey(
                        name: "FK_BookingProductImages_BookingProduct_productID",
                        column: x => x.productID,
                        principalSchema: "booking",
                        principalTable: "BookingProduct",
                        principalColumn: "productID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingProductImages_productID",
                table: "BookingProductImages",
                column: "productID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingProductImages");

            migrationBuilder.CreateTable(
                name: "BookingProductDescriptions",
                columns: table => new
                {
                    productDescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingProductDescriptions", x => x.productDescriptionID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingProductDetails_productDescriptionID",
                table: "BookingProductDetails",
                column: "productDescriptionID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingProductDetails_BookingProductDescriptions_productDescriptionID",
                table: "BookingProductDetails",
                column: "productDescriptionID",
                principalTable: "BookingProductDescriptions",
                principalColumn: "productDescriptionID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
