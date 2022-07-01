using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class updoadfilerequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "fileBukti",
                table: "leavingRequests",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "tipeFileBukti",
                table: "leavingRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileBukti",
                table: "leavingRequests");

            migrationBuilder.DropColumn(
                name: "tipeFileBukti",
                table: "leavingRequests");
        }
    }
}
