using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BM.WebAPI.Migrations
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
                name: "BookingPromotion",
                schema: "booking",
                columns: table => new
                {
                    promoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    promoName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    promoDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    promoDiscount = table.Column<double>(type: "float", nullable: false),
                    promoStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    promoEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    promoStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingPromotion", x => x.promoID);
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
                name: "BookingAppointment",
                schema: "booking",
                columns: table => new
                {
                    appID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    servID = table.Column<int>(type: "int", nullable: false),
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
                        name: "FK_BookingAppointment_BookingService_servID",
                        column: x => x.servID,
                        principalSchema: "booking",
                        principalTable: "BookingService",
                        principalColumn: "servID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingServPro",
                schema: "booking",
                columns: table => new
                {
                    servProID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    servID = table.Column<int>(type: "int", nullable: false),
                    promoID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingServPro", x => x.servProID);
                    table.ForeignKey(
                        name: "FK_BookingServPro_BookingPromotion_promoID",
                        column: x => x.promoID,
                        principalSchema: "booking",
                        principalTable: "BookingPromotion",
                        principalColumn: "promoID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingServPro_BookingService_servID",
                        column: x => x.servID,
                        principalSchema: "booking",
                        principalTable: "BookingService",
                        principalColumn: "servID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingReview",
                schema: "booking",
                columns: table => new
                {
                    reviewID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    appID = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    reviewContent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    reviewDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    reviewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingReview", x => x.reviewID);
                    table.ForeignKey(
                        name: "FK_BookingReview_BookingAppointment_appID",
                        column: x => x.appID,
                        principalSchema: "booking",
                        principalTable: "BookingAppointment",
                        principalColumn: "appID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingAppointment_orderID",
                schema: "booking",
                table: "BookingAppointment",
                column: "orderID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingAppointment_servID",
                schema: "booking",
                table: "BookingAppointment",
                column: "servID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingInvoice_orderID",
                schema: "booking",
                table: "BookingInvoice",
                column: "orderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingReview_appID",
                schema: "booking",
                table: "BookingReview",
                column: "appID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookingServPro_promoID",
                schema: "booking",
                table: "BookingServPro",
                column: "promoID");

            migrationBuilder.CreateIndex(
                name: "IX_BookingServPro_servID",
                schema: "booking",
                table: "BookingServPro",
                column: "servID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingInvoice",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingReview",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingServPro",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingAppointment",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingPromotion",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingOrder",
                schema: "booking");

            migrationBuilder.DropTable(
                name: "BookingService",
                schema: "booking");
        }
    }
}
