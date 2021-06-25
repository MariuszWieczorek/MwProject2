using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class UndoProjectStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_GetProjectStatuses_ProjectStatusId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GetProjectStatuses",
                table: "GetProjectStatuses");

            migrationBuilder.RenameTable(
                name: "GetProjectStatuses",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectStatus_ProjectStatusId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectStatus",
                table: "ProjectStatus");

            migrationBuilder.RenameTable(
                name: "ProjectStatus",
                newName: "GetProjectStatuses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GetProjectStatuses",
                table: "GetProjectStatuses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_GetProjectStatuses_ProjectStatusId",
                table: "Projects",
                column: "ProjectStatusId",
                principalTable: "GetProjectStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
