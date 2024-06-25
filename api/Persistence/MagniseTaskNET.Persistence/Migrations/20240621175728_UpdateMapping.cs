using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagniseTaskNET.Persistence.Migrations
{
    public partial class UpdateMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MappingType",
                table: "Mappings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MappingType",
                table: "Mappings");
        }
    }
}
