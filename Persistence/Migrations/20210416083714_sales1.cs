using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Data.Migrations
{
    public partial class sales1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcceptedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDate",
                table: "Projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "acceptedDate",
                table: "Projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "confirmedBy",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "confirmedBy",
                table: "Calculations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "confirmedDate",
                table: "Calculations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "createdBy",
                table: "Calculations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createdDate",
                table: "Calculations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "EstimatedSalesValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<byte>(type: "tinyint", nullable: false),
                    qty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstimatedSalesValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstimatedSalesValue_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstimatedSalesValue_ProjectId",
                table: "EstimatedSalesValue",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstimatedSalesValue");

            migrationBuilder.DropColumn(
                name: "AcceptedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ConfirmedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "acceptedDate",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "confirmedBy",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "confirmedBy",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "confirmedDate",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "createdBy",
                table: "Calculations");

            migrationBuilder.DropColumn(
                name: "createdDate",
                table: "Calculations");
        }
    }
}
