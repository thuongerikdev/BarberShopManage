using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BM.WebAPI.Migrations.BookingDb
{
    /// <inheritdoc />
    public partial class BookingV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "booking");

            migrationBuilder.CreateTable(
                name: "BookingCategories",
                columns: table => new
                {
                    categoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryPrice = table.Column<double>(type: "float", nullable: false),
                    categoryStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingCategories", x => x.categoryID);
                });

            migrationBuilder.CreateTable(
                name: "BookingOrder",
                schema: "booking",
                columns: table => new
                {
                    orderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    custID = table.Column<int>(type: "int", nullable: false),
                    orderTotal = table.Column<double>(type: "float", nullable: false),
                    orderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    orderDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingOrder", x => x.orderID);
                });

            migrationBuilder.CreateTable(
                name: "BookingProductDescriptions",
                columns: table => new
                {
                    productDescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingProductDescriptions", x => x.productDescriptionID);
                });

            migrationBuilder.CreateTable(
                name: "BookingService",
                schema: "booking",
                columns: table => new
                {
                    servID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    servName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    servDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    servPrice = table.Column<double>(type: "float", nullable: false),
                    servStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    servImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingService", x => x.servID);
                });

            migrationBuilder.CreateTable(
                name: "BookingSuppliers",
                columns: table => new
                {
                    supplierID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    supplierName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierNote = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplierImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingSuppliers", x => x.supplierID);
                });

            migrationBuilder.CreateTable(
                name: "BookingProduct",
                schema: "booking",
                columns: table => new
                {
                    productID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productPrice = table.Column<double>(type: "float", nullable: false),
                    productStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingProduct", x => x.productID);
                    table.ForeignKey(
                        name: "FK_BookingProduct_BookingCategories_categoryID",
                        column: x => x.categoryID,
                        principalTable: "BookingCategories",
                        principalColumn: "categoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingInvoice",
                schema: "booking",
                columns: table => new
                {
                    invoiceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderID = table.Column<int>(type: "int", nullable: false),
                    invoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    totalAmount = table.Column<double>(type: "float", nullable: false),
                    paymentTerms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingInvoice", x => x.invoiceID);
                    table.ForeignKey(
                        name: "FK_BookingInvoice_BookingOrder_orderID",
                        column: x => x.orderID,
                        principalSchema: "booking",
                        principalTable: "BookingOrder",
                        principalColumn: "orderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingReview",
                schema: "booking",
                columns: table => new
                {
                    reviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderID = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    reviewContent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    reviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reviewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingReview", x => x.reviewID);
                    table.ForeignKey(
                        name: "FK_BookingReview_BookingOrder_orderID",
                        column: x => x.orderID,
                        principalSchema: "booking",
                        principalTable: "BookingOrder",
                        principalColumn: "orderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingServiceDetails",
                columns: table => new
                {
                    serviceDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    servID = table.Column<int>(type: "int", nullable: false),
                    servPrice = table.Column<double>(type: "float", nullable: false),
                    servImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    servName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    servDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    servStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingServiceDetails", x => x.serviceDetailID);
                    table.ForeignKey(
                        name: "FK_BookingServiceDetails_BookingService_servID",
                        column: x => x.servID,
                        principalSchema: "booking",
                        principalTable: "BookingService",
                        principalColumn: "servID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingProductDetails",
                columns: table => new
                {
                    productDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productID = table.Column<int>(type: "int", nullable: false),
                    productDescriptionID = table.Column<int>(type: "int", nullable: false),
                    supplierID = table.Column<int>(type: "int", nullable: false),
                    productPrice = table.Column<double>(type: "float", nullable: false),
                    productQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingProductDetails", x => x.productDetailID);
                    table.ForeignKey(
                        name: "FK_BookingProductDetails_BookingProductDescriptions_productDescriptionID",
                        column: x => x.productDescriptionID,
                        principalTable: "BookingProductDescriptions",
                        principalColumn: "productDescriptionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingProductDetails_BookingProduct_productID",
                        column: x => x.productID,
                        principalSchema: "booking",
                        principalTable: "BookingProduct",
                        principalColumn: "productID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingProductDetails_BookingSuppliers_supplierID",
                        column: x => x.supplierID,
                        principalTable: "BookingSuppliers",
                        principalColumn: "supplierID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingAppointment",
                schema: "booking",
                columns: table => new
                {
                    appID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    serviceDetailID = table.Column<int>(type: "int", nullable: false),
                    empID = table.Column<int>(type: "int", nullable: false),
                    orderID = table.Column<int>(type: "int", nullable: false),
                    appStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingAppointment", x => x.appID);
                    table.ForeignKey(
                        name: "FK_BookingAppointment_BookingOrder_orderID",
                        column: x => x.orderID,
                        principalSchema: "booking",
                        principalTable: "BookingOrder",
                        principalColumn: "orderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingAppointment_BookingServiceDetails_serviceDetailID",
                        column: x => x.serviceDetailID,
                        principalTable: "BookingServiceDetails",
                        principalColumn: "serviceDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingOrderProducts",
                columns: table => new
                {
                    orderProductID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderID = table.Column<int>(type: "int", nullable: false),
                    productDetailID = table.Column<int>(type: "int", nullable: false),
                    productPrice = table.Column<double>(type: "float", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingOrderProducts", x => x.orderProductID);
                    table.ForeignKey(
                        name: "FK_BookingOrderProducts_BookingOrder_orderID",
                        column: x => x.orderID,
                        principalSchema: "booking",
                        principalTable: "BookingOrder",
                        principalColumn: "orderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingOrderProducts_BookingProductDetails_productDetailID",
                        column: x => x.productDetailID,
                        principalTable: "BookingProductDetails",
                        principalColumn: "productDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingAppointment_orderID",
                schema: "booking",
                table: "BookingAppointment",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingAppointment_serviceDetailID",
                schema: "booking",
                table: "BookingAppointment",
                column: "serviceDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingInvoice_orderID",
                schema: "booking",
                table: "BookingInvoice",
                column: "orderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrderProducts_orderID",
                table: "BookingOrderProducts",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingOrderProducts_productDetailID",
                table: "BookingOrderProducts",
                column: "productDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingProduct_categoryID",
                schema: "booking",
                table: "BookingProduct",
                column: "categoryID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingProductDetails_productDescriptionID",
                table: "BookingProductDetails",
                column: "productDescriptionID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingProductDetails_productID",
                table: "BookingProductDetails",
                column: "productID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingProductDetails_supplierID",
                table: "BookingProductDetails",
                column: "supplierID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingReview_orderID",
                schema: "booking",
                table: "BookingReview",
                column: "orderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingServiceDetails_servID",
                table: "BookingServiceDetails",
                column: "servID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingAppointment",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingInvoice",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingOrderProducts");

            migrationBuilder.DropTable(
                name: "BookingReview",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingServiceDetails");

            migrationBuilder.DropTable(
                name: "BookingProductDetails");

            migrationBuilder.DropTable(
                name: "BookingOrder",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingService",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingProductDescriptions");

            migrationBuilder.DropTable(
                name: "BookingProduct",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingSuppliers");

            migrationBuilder.DropTable(
                name: "BookingCategories");
        }
    }
}
