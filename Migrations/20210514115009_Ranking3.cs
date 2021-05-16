using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class Ranking3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rangeTo",
                table: "RankingElements",
                newName: "RangeTo");

            migrationBuilder.RenameColumn(
                name: "rangeFrom",
                table: "RankingElements",
                newName: "RangeFrom");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RankingElements",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "RankingElements");

            migrationBuilder.RenameColumn(
                name: "RangeTo",
                table: "RankingElements",
                newName: "rangeTo");

            migrationBuilder.RenameColumn(
                name: "RangeFrom",
                table: "RankingElements",
                newName: "rangeFrom");
        }
    }
}
