using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class ProjectTeemMemberUserIdIntToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeamMembers_AspNetUsers_UserId1",
                table: "ProjectTeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTeamMembers_UserId1",
                table: "ProjectTeamMembers");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "ProjectTeamMembers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectTeamMembers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamMembers_UserId",
                table: "ProjectTeamMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeamMembers_AspNetUsers_UserId",
                table: "ProjectTeamMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeamMembers_AspNetUsers_UserId",
                table: "ProjectTeamMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTeamMembers_UserId",
                table: "ProjectTeamMembers");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ProjectTeamMembers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "ProjectTeamMembers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamMembers_UserId1",
                table: "ProjectTeamMembers",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeamMembers_AspNetUsers_UserId1",
                table: "ProjectTeamMembers",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
