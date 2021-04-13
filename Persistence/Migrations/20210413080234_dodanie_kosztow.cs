using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Data.Migrations
{
    public partial class dodanie_kosztow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ckw",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CommentofCost",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GeneralCostsInPercent",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LabourCosts",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Markup",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MaterialCosts",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PackingCosts",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Tkw",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ckw",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CommentofCost",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "GeneralCostsInPercent",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "LabourCosts",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Markup",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "MaterialCosts",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PackingCosts",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Tkw",
                table: "Projects");
        }
    }
}
