using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class ProjectTeemMember2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectTeamMembers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamMembers_ProjectId",
                table: "ProjectTeamMembers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeamMembers_Projects_ProjectId",
                table: "ProjectTeamMembers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeamMembers_Projects_ProjectId",
                table: "ProjectTeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTeamMembers_ProjectId",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ProjectTeamMembers");
        }
    }
}
