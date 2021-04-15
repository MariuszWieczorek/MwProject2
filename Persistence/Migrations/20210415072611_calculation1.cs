﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Data.Migrations
{
    public partial class calculation1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Calculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    MaterialCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LabourCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Markup = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PackingCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GeneralCostsInPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tkw = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ckw = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calculations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calculations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calculations_ProjectId",
                table: "Calculations",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calculations");

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
    }
}
