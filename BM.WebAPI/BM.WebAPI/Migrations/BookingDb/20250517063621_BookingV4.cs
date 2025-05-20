using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BM.WebAPI.Migrations.BookingDb
{
    /// <inheritdoc />
    public partial class BookingV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bookingServiceDetailDescriptions",
                columns: table => new
                {
                    serviceDetailDescriptionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    serviceDetailID = table.Column<int>(type: "int", nullable: false),
                    servImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    servName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    servDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    servStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    servType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookingServiceDetailDescriptions", x => x.serviceDetailDescriptionID);
                    table.ForeignKey(
                        name: "FK_bookingServiceDetailDescriptions_BookingServiceDetails_serviceDetailID",
                        column: x => x.serviceDetailID,
                        principalTable: "BookingServiceDetails",
                        principalColumn: "serviceDetailID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookingServiceDetailDescriptions_serviceDetailID",
                table: "bookingServiceDetailDescriptions",
                column: "serviceDetailID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookingServiceDetailDescriptions");
        }
    }
}
