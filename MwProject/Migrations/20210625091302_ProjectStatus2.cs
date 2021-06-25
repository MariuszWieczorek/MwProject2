using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class ProjectStatus2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectStatus_ProjectStatusId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStatus",
                table: "ProjectStatus");

            migrationBuilder.RenameTable(
                name: "ProjectStatus",
                newName: "ProjectStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStatuses",
                table: "ProjectStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectStatuses_ProjectStatusId",
                table: "Projects",
                column: "ProjectStatusId",
                principalTable: "ProjectStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectStatuses_ProjectStatusId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStatuses",
                table: "ProjectStatuses");

            migrationBuilder.RenameTable(
                name: "ProjectStatuses",
                newName: "ProjectStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectStatus",
                table: "ProjectStatus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectStatus_ProjectStatusId",
                table: "Projects",
                column: "ProjectStatusId",
                principalTable: "ProjectStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
