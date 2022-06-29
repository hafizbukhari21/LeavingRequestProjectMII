using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "divisi",
                columns: table => new
                {
                    divisi_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    namaDivisi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_divisi", x => x.divisi_id);
                });

            migrationBuilder.CreateTable(
                name: "leaveCategories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nameCategory = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leaveCategories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    roleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    employee_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    gender = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sisaCuti = table.Column<int>(type: "int", nullable: false),
                    role_Id = table.Column<int>(type: "int", nullable: false),
                    manager_id = table.Column<int>(type: "int", nullable: false),
                    divisi_id = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_employees", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_employees_divisi_divisi_id",
                        column: x => x.divisi_id,
                        principalTable: "divisi",
                        principalColumn: "divisi_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_employees_roles_role_Id",
                        column: x => x.role_Id,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "leavingRequests",
                columns: table => new
                {
                    request_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    employee_id = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    category_id = table.Column<int>(type: "int", nullable: false),
                    approvalStatus = table.Column<int>(type: "int", nullable: false),
                    requestTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    approvalMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leavingRequests", x => x.request_id);
                    table.ForeignKey(
                        name: "FK_leavingRequests_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_leavingRequests_leaveCategories_category_id",
                        column: x => x.category_id,
                        principalTable: "leaveCategories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_employees_divisi_id",
                table: "employees",
                column: "divisi_id");

            migrationBuilder.CreateIndex(
                name: "IX_employees_role_Id",
                table: "employees",
                column: "role_Id");

            migrationBuilder.CreateIndex(
                name: "IX_leavingRequests_category_id",
                table: "leavingRequests",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_leavingRequests_employee_id",
                table: "leavingRequests",
                column: "employee_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "leavingRequests");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "leaveCategories");

            migrationBuilder.DropTable(
                name: "divisi");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
