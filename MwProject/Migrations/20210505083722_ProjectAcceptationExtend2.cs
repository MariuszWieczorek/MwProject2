using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class ProjectAcceptationExtend2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EconomicRequirementsConfirmedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EconomicRequirementsConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEconomicRequirementsConfirmed",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsQualityRequirementsConfirmed",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTechnicalProportiesConfirmed",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QualityRequirementsConfirmedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QualityRequirementsConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TechnicalProportiesConfirmedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TechnicalProportiesConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EconomicRequirementsConfirmedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "EconomicRequirementsConfirmedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsEconomicRequirementsConfirmed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsQualityRequirementsConfirmed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsTechnicalProportiesConfirmed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "QualityRequirementsConfirmedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "QualityRequirementsConfirmedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TechnicalProportiesConfirmedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TechnicalProportiesConfirmedDate",
                table: "Projects");
        }
    }
}
