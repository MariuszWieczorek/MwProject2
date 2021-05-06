using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class UserRights : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdminRights",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanAcceptProject",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanConfirmCalculations",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanConfirmEconomicRequirements",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanConfirmEstimatedSales",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanConfirmProject",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanConfirmQualityRequirements",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanConfirmTechnicalProperties",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanModifyProject",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminRights",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanAcceptProject",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanConfirmCalculations",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanConfirmEconomicRequirements",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanConfirmEstimatedSales",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanConfirmProject",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanConfirmQualityRequirements",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanConfirmTechnicalProperties",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanModifyProject",
                table: "AspNetUsers");
        }
    }
}
