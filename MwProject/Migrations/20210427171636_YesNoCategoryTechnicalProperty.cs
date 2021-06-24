using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class YesNoCategoryTechnicalProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowValue",
                table: "ProjectTechnicalProperties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowValue",
                table: "CategoryTechnicalProperties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "YesNo",
                table: "CategoryTechnicalProperties",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowValue",
                table: "ProjectTechnicalProperties");

            migrationBuilder.DropColumn(
                name: "ShowValue",
                table: "CategoryTechnicalProperties");

            migrationBuilder.DropColumn(
                name: "YesNo",
                table: "CategoryTechnicalProperties");
        }
    }
}
