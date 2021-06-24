using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class CalculationAddedTitleAndOrdinalNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedDate",
                table: "Calculations");

            migrationBuilder.RenameColumn(
                name: "ConfirmedBy",
                table: "Calculations",
                newName: "Title");

            migrationBuilder.AddColumn<int>(
                name: "OrdinalNumber",
                table: "Calculations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrdinalNumber",
                table: "Calculations");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Calculations",
                newName: "ConfirmedBy");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDate",
                table: "Calculations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
