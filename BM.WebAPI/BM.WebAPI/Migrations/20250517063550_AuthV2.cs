using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BM.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AuthV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "percentDiscount",
                schema: "auth",
                table: "AuthCustomer");

            migrationBuilder.AddColumn<int>(
                name: "pointToGet",
                table: "Promos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "createAt",
                table: "CusPromos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "promoCount",
                table: "CusPromos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "updateAt",
                table: "CusPromos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "customerCheckIns",
                columns: table => new
                {
                    customerCheckInID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customerID = table.Column<int>(type: "int", nullable: false),
                    checkInDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    checkInStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    checkInType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerCheckIns", x => x.customerCheckInID);
                    table.ForeignKey(
                        name: "FK_customerCheckIns_AuthCustomer_customerID",
                        column: x => x.customerID,
                        principalSchema: "auth",
                        principalTable: "AuthCustomer",
                        principalColumn: "customerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customerCheckIns_customerID",
                table: "customerCheckIns",
                column: "customerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customerCheckIns");

            migrationBuilder.DropColumn(
                name: "pointToGet",
                table: "Promos");

            migrationBuilder.DropColumn(
                name: "createAt",
                table: "CusPromos");

            migrationBuilder.DropColumn(
                name: "promoCount",
                table: "CusPromos");

            migrationBuilder.DropColumn(
                name: "updateAt",
                table: "CusPromos");

            migrationBuilder.AddColumn<double>(
                name: "percentDiscount",
                schema: "auth",
                table: "AuthCustomer",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
