﻿using System;
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
                name: "AuthPermission",
                schema: "auth",
                columns: table => new
                {
                    permissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    permissionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    permissionDes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthPermission", x => x.permissionID);
                });

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
                name: "AuthRole",
                schema: "auth",
                columns: table => new
                {
                    roleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    roleDes = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthRole", x => x.roleID);
                });

            migrationBuilder.CreateTable(
                name: "AuthSchedule",
                schema: "auth",
                columns: table => new
                {
                    scheduleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    scheduleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    percent = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthSchedule", x => x.scheduleID);
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
                name: "Branches",
                columns: table => new
                {
                    branchID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    branchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    branchType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    branchStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    branchArea = table.Column<double>(type: "float", nullable: false),
                    branchHotline = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    startWork = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endWork = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    branchImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.branchID);
                });

            migrationBuilder.CreateTable(
                name: "Promos",
                columns: table => new
                {
                    promoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    promoName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    promoDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    promoDiscount = table.Column<double>(type: "float", nullable: false),
                    promoStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    promoEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    promoStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    promoType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    promoImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promos", x => x.promoID);
                });

            migrationBuilder.CreateTable(
                name: "Vips",
                columns: table => new
                {
                    vipID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vipType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vipStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vipCost = table.Column<double>(type: "float", nullable: false),
                    vipDiscount = table.Column<double>(type: "float", nullable: false),
                    vipImage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vips", x => x.vipID);
                });

            migrationBuilder.CreateTable(
                name: "AuthRolePermission",
                schema: "auth",
                columns: table => new
                {
                    rolePermissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleID = table.Column<int>(type: "int", nullable: false),
                    permissionID = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthRolePermission", x => x.rolePermissionID);
                    table.ForeignKey(
                        name: "FK_AuthRolePermission_AuthPermission_permissionID",
                        column: x => x.permissionID,
                        principalSchema: "auth",
                        principalTable: "AuthPermission",
                        principalColumn: "permissionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthRolePermission_AuthRole_roleID",
                        column: x => x.roleID,
                        principalSchema: "auth",
                        principalTable: "AuthRole",
                        principalColumn: "roleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthUser",
                schema: "auth",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleID = table.Column<int>(type: "int", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    passwordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordSalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    phoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isEmp = table.Column<bool>(type: "bit", nullable: false),
                    dateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    isEmailVerified = table.Column<bool>(type: "bit", nullable: false),
                    emailVerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUser", x => x.userID);
                    table.ForeignKey(
                        name: "FK_AuthUser_AuthRole_roleID",
                        column: x => x.roleID,
                        principalSchema: "auth",
                        principalTable: "AuthRole",
                        principalColumn: "roleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthCustomer",
                schema: "auth",
                columns: table => new
                {
                    customerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: false),
                    vipID = table.Column<int>(type: "int", nullable: false),
                    loyaltyPoints = table.Column<double>(type: "float", nullable: false),
                    customerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customerStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    totalSpent = table.Column<double>(type: "float", nullable: false),
                    percentDiscount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthCustomer", x => x.customerID);
                    table.ForeignKey(
                        name: "FK_AuthCustomer_AuthUser_userID",
                        column: x => x.userID,
                        principalSchema: "auth",
                        principalTable: "AuthUser",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthCustomer_Vips_vipID",
                        column: x => x.vipID,
                        principalTable: "Vips",
                        principalColumn: "vipID",
                        onDelete: ReferentialAction.Cascade);
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
                    empCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    salary = table.Column<double>(type: "float", nullable: false),
                    bonusSalary = table.Column<double>(type: "float", nullable: false),
                    rate = table.Column<double>(type: "float", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    branchID = table.Column<int>(type: "int", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_AuthEmp_Branches_branchID",
                        column: x => x.branchID,
                        principalTable: "Branches",
                        principalColumn: "branchID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CusPromos",
                columns: table => new
                {
                    cusPromoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customerID = table.Column<int>(type: "int", nullable: false),
                    promoID = table.Column<int>(type: "int", nullable: false),
                    cusPromoStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CusPromos", x => x.cusPromoID);
                    table.ForeignKey(
                        name: "FK_CusPromos_AuthCustomer_customerID",
                        column: x => x.customerID,
                        principalSchema: "auth",
                        principalTable: "AuthCustomer",
                        principalColumn: "customerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CusPromos_Promos_promoID",
                        column: x => x.promoID,
                        principalTable: "Promos",
                        principalColumn: "promoID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthScheEmp",
                schema: "auth",
                columns: table => new
                {
                    scheEmpID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    empID = table.Column<int>(type: "int", nullable: false),
                    scheduleID = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthScheEmp", x => x.scheEmpID);
                    table.ForeignKey(
                        name: "FK_AuthScheEmp_AuthEmp_empID",
                        column: x => x.empID,
                        principalSchema: "auth",
                        principalTable: "AuthEmp",
                        principalColumn: "empID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthScheEmp_AuthSchedule_scheduleID",
                        column: x => x.scheduleID,
                        principalSchema: "auth",
                        principalTable: "AuthSchedule",
                        principalColumn: "scheduleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthCustomer_userID",
                schema: "auth",
                table: "AuthCustomer",
                column: "userID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthCustomer_vipID",
                schema: "auth",
                table: "AuthCustomer",
                column: "vipID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthEmp_branchID",
                schema: "auth",
                table: "AuthEmp",
                column: "branchID");

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

            migrationBuilder.CreateIndex(
                name: "IX_AuthRolePermission_permissionID",
                schema: "auth",
                table: "AuthRolePermission",
                column: "permissionID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthRolePermission_roleID",
                schema: "auth",
                table: "AuthRolePermission",
                column: "roleID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthScheEmp_empID",
                schema: "auth",
                table: "AuthScheEmp",
                column: "empID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthScheEmp_scheduleID",
                schema: "auth",
                table: "AuthScheEmp",
                column: "scheduleID");

            migrationBuilder.CreateIndex(
                name: "IX_AuthUser_roleID",
                schema: "auth",
                table: "AuthUser",
                column: "roleID");

            migrationBuilder.CreateIndex(
                name: "IX_CusPromos_customerID",
                table: "CusPromos",
                column: "customerID");

            migrationBuilder.CreateIndex(
                name: "IX_CusPromos_promoID",
                table: "CusPromos",
                column: "promoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthRolePermission",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthScheEmp",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "CusPromos");

            migrationBuilder.DropTable(
                name: "AuthPermission",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthEmp",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthSchedule",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthCustomer",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Promos");

            migrationBuilder.DropTable(
                name: "AuthPosition",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "AuthSpecialty",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropTable(
                name: "AuthUser",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "Vips");

            migrationBuilder.DropTable(
                name: "AuthRole",
                schema: "auth");
        }
    }
}
