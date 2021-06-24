using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class YesNoProjectTechnicalProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categoryTechnicalProperties_Categories_CategoryId",
                table: "categoryTechnicalProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_categoryTechnicalProperties_TechnicalProperties_TechnicalPropertyId",
                table: "categoryTechnicalProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categoryTechnicalProperties",
                table: "categoryTechnicalProperties");

            migrationBuilder.RenameTable(
                name: "categoryTechnicalProperties",
                newName: "CategoryTechnicalProperties");

            migrationBuilder.RenameIndex(
                name: "IX_categoryTechnicalProperties_TechnicalPropertyId",
                table: "CategoryTechnicalProperties",
                newName: "IX_CategoryTechnicalProperties_TechnicalPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_categoryTechnicalProperties_CategoryId",
                table: "CategoryTechnicalProperties",
                newName: "IX_CategoryTechnicalProperties_CategoryId");

            migrationBuilder.AddColumn<byte>(
                name: "YesNo",
                table: "ProjectTechnicalProperties",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryTechnicalProperties",
                table: "CategoryTechnicalProperties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTechnicalProperties_Categories_CategoryId",
                table: "CategoryTechnicalProperties",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryTechnicalProperties_TechnicalProperties_TechnicalPropertyId",
                table: "CategoryTechnicalProperties",
                column: "TechnicalPropertyId",
                principalTable: "TechnicalProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTechnicalProperties_Categories_CategoryId",
                table: "CategoryTechnicalProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryTechnicalProperties_TechnicalProperties_TechnicalPropertyId",
                table: "CategoryTechnicalProperties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryTechnicalProperties",
                table: "CategoryTechnicalProperties");

            migrationBuilder.DropColumn(
                name: "YesNo",
                table: "ProjectTechnicalProperties");

            migrationBuilder.RenameTable(
                name: "CategoryTechnicalProperties",
                newName: "categoryTechnicalProperties");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryTechnicalProperties_TechnicalPropertyId",
                table: "categoryTechnicalProperties",
                newName: "IX_categoryTechnicalProperties_TechnicalPropertyId");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryTechnicalProperties_CategoryId",
                table: "categoryTechnicalProperties",
                newName: "IX_categoryTechnicalProperties_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categoryTechnicalProperties",
                table: "categoryTechnicalProperties",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_categoryTechnicalProperties_Categories_CategoryId",
                table: "categoryTechnicalProperties",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_categoryTechnicalProperties_TechnicalProperties_TechnicalPropertyId",
                table: "categoryTechnicalProperties",
                column: "TechnicalPropertyId",
                principalTable: "TechnicalProperties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
