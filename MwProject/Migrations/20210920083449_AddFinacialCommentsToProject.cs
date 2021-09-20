using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddFinacialCommentsToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FinancialComments",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialComments",
                table: "Projects");
        }
    }
}
