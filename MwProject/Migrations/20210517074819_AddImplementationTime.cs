using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddImplementationTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImplementationTimeInMonths",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImplementationTimeInMonthsRanking",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedEndDateOfTheProject",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedStartDateOfTheProject",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RealStartDateOfTheProject",
                table: "Projects",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImplementationTimeInMonths",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ImplementationTimeInMonthsRanking",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PlannedEndDateOfTheProject",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PlannedStartDateOfTheProject",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RealStartDateOfTheProject",
                table: "Projects");
        }
    }
}
