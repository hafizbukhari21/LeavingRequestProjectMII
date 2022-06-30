using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class messagecuti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "leavingMessage",
                table: "leavingRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "leavingMessage",
                table: "leavingRequests");
        }
    }
}
