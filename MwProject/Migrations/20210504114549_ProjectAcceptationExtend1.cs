using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class ProjectAcceptationExtend1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CalculationConfirmedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CalculationConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EstimatedSalesConfirmedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedSalesConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCalculationConfirmed",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEstimatedSalesConfirmed",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculationConfirmedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CalculationConfirmedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "EstimatedSalesConfirmedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "EstimatedSalesConfirmedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsCalculationConfirmed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsEstimatedSalesConfirmed",
                table: "Projects");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
