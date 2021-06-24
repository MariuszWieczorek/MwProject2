using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddFirstThreeRankingsToProject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompetitivenessOfTheProjectId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurposeOfTheProjectId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViabilityOfTheProjectId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CompetitivenessOfTheProjectId",
                table: "Projects",
                column: "CompetitivenessOfTheProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PurposeOfTheProjectId",
                table: "Projects",
                column: "PurposeOfTheProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ViabilityOfTheProjectId",
                table: "Projects",
                column: "ViabilityOfTheProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_RankingElements_CompetitivenessOfTheProjectId",
                table: "Projects",
                column: "CompetitivenessOfTheProjectId",
                principalTable: "RankingElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_RankingElements_PurposeOfTheProjectId",
                table: "Projects",
                column: "PurposeOfTheProjectId",
                principalTable: "RankingElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_RankingElements_ViabilityOfTheProjectId",
                table: "Projects",
                column: "ViabilityOfTheProjectId",
                principalTable: "RankingElements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_RankingElements_CompetitivenessOfTheProjectId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_RankingElements_PurposeOfTheProjectId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_RankingElements_ViabilityOfTheProjectId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CompetitivenessOfTheProjectId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_PurposeOfTheProjectId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ViabilityOfTheProjectId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "CompetitivenessOfTheProjectId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PurposeOfTheProjectId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ViabilityOfTheProjectId",
                table: "Projects");
        }
    }
}
