using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class namafileBuktiAndisRead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isRead",
                table: "leavingRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "namaFileBukti",
                table: "leavingRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isRead",
                table: "leavingRequests");

            migrationBuilder.DropColumn(
                name: "namaFileBukti",
                table: "leavingRequests");
        }
    }
}
