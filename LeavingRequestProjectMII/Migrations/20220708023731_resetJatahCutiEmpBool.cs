using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class resetJatahCutiEmpBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "resetCutiAkhirTahun",
                table: "employees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resetCutiAkhirTahun",
                table: "employees");
        }
    }
}
