using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class UserRights3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanEditCalculations",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditEconomicRequirements",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditEstimatedSales",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditQualityRequirements",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditTechnicalProperties",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanEditCalculations",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanEditEconomicRequirements",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanEditEstimatedSales",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanEditQualityRequirements",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanEditTechnicalProperties",
                table: "AspNetUsers");
        }
    }
}
