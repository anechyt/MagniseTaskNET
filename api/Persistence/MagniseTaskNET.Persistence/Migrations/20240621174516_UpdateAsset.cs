using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagniseTaskNET.Persistence.Migrations
{
    public partial class UpdateAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime",
                table: "Assets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateTime",
                table: "Assets");
        }
    }
}
