using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddRelatedNumbersAndLinkToProjectReq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "CategoryRequirements");

            migrationBuilder.DropColumn(
                name: "RelatedNumbers",
                table: "CategoryRequirements");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "ProjectRequirements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedNumbers",
                table: "ProjectRequirements",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "ProjectRequirements");

            migrationBuilder.DropColumn(
                name: "RelatedNumbers",
                table: "ProjectRequirements");

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "CategoryRequirements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedNumbers",
                table: "CategoryRequirements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
