using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddWzp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReturnOnInvestmentRanking",
                table: "Projects",
                newName: "RankingOfUsedProductionCapability");

            migrationBuilder.RenameColumn(
                name: "ImplementationTimeInMonthsRanking",
                table: "Projects",
                newName: "RankingOfReturnOnInvestment");

            migrationBuilder.RenameColumn(
                name: "EstimatedPaybackTimeInMonthsRanking",
                table: "Projects",
                newName: "RankingOfImplementationTimeInMonths");

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageOfUsedProductionCapability",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PlannedProductionVolume",
                table: "Projects",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ProductionCapacity",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RankingOfEstimatedPaybackTimeInMonths",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentageOfUsedProductionCapability",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PlannedProductionVolume",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProductionCapacity",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RankingOfEstimatedPaybackTimeInMonths",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "RankingOfUsedProductionCapability",
                table: "Projects",
                newName: "ReturnOnInvestmentRanking");

            migrationBuilder.RenameColumn(
                name: "RankingOfReturnOnInvestment",
                table: "Projects",
                newName: "ImplementationTimeInMonthsRanking");

            migrationBuilder.RenameColumn(
                name: "RankingOfImplementationTimeInMonths",
                table: "Projects",
                newName: "EstimatedPaybackTimeInMonthsRanking");
        }
    }
}
