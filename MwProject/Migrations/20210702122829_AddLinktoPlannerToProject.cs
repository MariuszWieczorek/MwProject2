using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddLinktoPlannerToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_projectGroups_ProjectGroupId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_projectGroups",
                table: "projectGroups");

            migrationBuilder.RenameTable(
                name: "projectGroups",
                newName: "ProjectGroups");

            migrationBuilder.AddColumn<string>(
                name: "LinkToPlanner",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectGroups",
                table: "ProjectGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectGroups_ProjectGroupId",
                table: "Projects",
                column: "ProjectGroupId",
                principalTable: "ProjectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectGroups_ProjectGroupId",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectGroups",
                table: "ProjectGroups");

            migrationBuilder.DropColumn(
                name: "LinkToPlanner",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "ProjectGroups",
                newName: "projectGroups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_projectGroups",
                table: "projectGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_projectGroups_ProjectGroupId",
                table: "Projects",
                column: "ProjectGroupId",
                principalTable: "projectGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
