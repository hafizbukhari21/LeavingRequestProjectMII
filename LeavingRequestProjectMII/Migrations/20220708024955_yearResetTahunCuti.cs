using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class yearResetTahunCuti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "resettahunCuti",
                table: "employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "resettahunCuti",
                table: "employees");
        }
    }
}
