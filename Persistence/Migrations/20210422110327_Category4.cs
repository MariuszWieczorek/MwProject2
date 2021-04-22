using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Data.Migrations
{
    public partial class Category4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentSymbol",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentSymbol",
                table: "Categories");
        }
    }
}
