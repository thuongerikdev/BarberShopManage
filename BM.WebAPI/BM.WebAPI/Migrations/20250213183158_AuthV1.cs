using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BM.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AuthV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "AuthPosition",
                schema: "auth",
                columns: table => new
                {
                    positionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    positionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DefaultSalary = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthPosition", x => x.positionID);
                });

            migrationBuilder.CreateTable(
                name: "AuthSpecialty",
                schema: "auth",
                columns: table => new
                {
                    specialtyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    specialtyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    note = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    percent = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthSpecialty", x => x.specialtyID);
                });

            migrationBuilder.CreateTable(
                name: "AuthUser",
                schema: "auth",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    email = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUser", x => x.userID);
                });

            migrationBuilder.CreateTable(
                name: "AuthEmp",
                schema: "auth",
                columns: table => new
                {
                    empID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: false),
                    positionID = table.Column<int>(type: "int", nullable: false),
                    specialtyID = table.Column<int>(type: "int", nullable: false),
                    empCode = table.Column<int>(type: "int", nullable: false),
                    salary = table.Column<double>(type: "float", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthEmp", x => x.empID);
                    table.ForeignKey(
                        name: "FK_AuthEmp_AuthPosition_positionID",
                        column: x => x.positionID,
                        principalSchema: "auth",
                        principalTable: "AuthPosition",
                        principalColumn: "positionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthEmp_AuthSpecialty_specialtyID",
                        column: x => x.specialtyID,
                        principalSchema: "auth",
                        principalTable: "AuthSpecialty",
                        principalColumn: "specialtyID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthEmp_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthEmp_positionID",
                schema: "auth",
                table: "AuthEmp",
                column: "positionID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthEmp_specialtyID",
                schema: "auth",
                table: "AuthEmp",
                column: "specialtyID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthEmp_userID",
                schema: "auth",
                table: "AuthEmp",
                column: "userID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthEmp",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthPosition",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthSpecialty",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthUser",
                schema: "auth");
        }
    }
}
