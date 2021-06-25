using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class ProjectStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectStatusId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GetProjectStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdinalNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbrev = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetProjectStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectStatusId",
                table: "Projects",
                column: "ProjectStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_GetProjectStatuses_ProjectStatusId",
                table: "Projects",
                column: "ProjectStatusId",
                principalTable: "GetProjectStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_GetProjectStatuses_ProjectStatusId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "GetProjectStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectStatusId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectStatusId",
                table: "Projects");
        }
    }
}
