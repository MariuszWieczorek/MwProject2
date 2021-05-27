using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class ProjectTeemMemberDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProjectTeamMembers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsProjectTeamConfirmed",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ProjectTeamConfirmedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjectTeamConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CanConfirmProjectTeam",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanEditProjectTeam",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "IsProjectTeamConfirmed",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectTeamConfirmedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectTeamConfirmedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CanConfirmProjectTeam",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CanEditProjectTeam",
                table: "AspNetUsers");
        }
    }
}
