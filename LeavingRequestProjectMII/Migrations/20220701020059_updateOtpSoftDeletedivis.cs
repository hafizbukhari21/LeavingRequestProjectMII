using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class updateOtpSoftDeletedivis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "expired",
                table: "employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isActiveOtp",
                table: "employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "tokenOtp",
                table: "employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "divisi",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "expired",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "isActiveOtp",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "tokenOtp",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "divisi");
        }
    }
}
