using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class CategoryRequirements2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryRequirements_Projects_ProjectId",
                table: "CategoryRequirements");

            migrationBuilder.DropIndex(
                name: "IX_CategoryRequirements_ProjectId",
                table: "CategoryRequirements");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                table: "CategoryRequirements",
                newName: "OrdinalNumber");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "CategoryRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRequirements_CategoryId",
                table: "CategoryRequirements",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryRequirements_Categories_CategoryId",
                table: "CategoryRequirements",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryRequirements_Categories_CategoryId",
                table: "CategoryRequirements");

            migrationBuilder.DropIndex(
                name: "IX_CategoryRequirements_CategoryId",
                table: "CategoryRequirements");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "CategoryRequirements");

            migrationBuilder.RenameColumn(
                name: "OrdinalNumber",
                table: "CategoryRequirements",
                newName: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryRequirements_ProjectId",
                table: "CategoryRequirements",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryRequirements_Projects_ProjectId",
                table: "CategoryRequirements",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
