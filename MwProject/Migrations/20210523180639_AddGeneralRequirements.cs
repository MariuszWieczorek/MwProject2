using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddGeneralRequirements : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneralRequirementsConfirmedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GeneralRequirementsConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsGeneralRequirementsConfirmed",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanConfirmGeneralRequirements",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditGeneralRequirements",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SuperAdminRights",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneralRequirementsConfirmedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "GeneralRequirementsConfirmedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsGeneralRequirementsConfirmed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CanConfirmGeneralRequirements",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanEditGeneralRequirements",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SuperAdminRights",
                table: "AspNetUsers");
        }
    }
}
