using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddRequestInfoToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRequestConfirmed",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RequestConfirmedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRequestConfirmed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RequestConfirmedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RequestConfirmedDate",
                table: "Projects");
        }
    }
}
