using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Data.Migrations
{
    public partial class sales2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimatedSalesValue_Projects_ProjectId",
                table: "EstimatedSalesValue");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstimatedSalesValue",
                table: "EstimatedSalesValue");

            migrationBuilder.RenameTable(
                name: "EstimatedSalesValue",
                newName: "EstimatedSalesValues");

            migrationBuilder.RenameColumn(
                name: "qty",
                table: "EstimatedSalesValues",
                newName: "Qty");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "EstimatedSalesValues",
                newName: "Price");

            migrationBuilder.RenameIndex(
                name: "IX_EstimatedSalesValue_ProjectId",
                table: "EstimatedSalesValues",
                newName: "IX_EstimatedSalesValues_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstimatedSalesValues",
                table: "EstimatedSalesValues",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimatedSalesValues_Projects_ProjectId",
                table: "EstimatedSalesValues",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EstimatedSalesValues_Projects_ProjectId",
                table: "EstimatedSalesValues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstimatedSalesValues",
                table: "EstimatedSalesValues");

            migrationBuilder.RenameTable(
                name: "EstimatedSalesValues",
                newName: "EstimatedSalesValue");

            migrationBuilder.RenameColumn(
                name: "Qty",
                table: "EstimatedSalesValue",
                newName: "qty");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "EstimatedSalesValue",
                newName: "price");

            migrationBuilder.RenameIndex(
                name: "IX_EstimatedSalesValues_ProjectId",
                table: "EstimatedSalesValue",
                newName: "IX_EstimatedSalesValue_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstimatedSalesValue",
                table: "EstimatedSalesValue",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EstimatedSalesValue_Projects_ProjectId",
                table: "EstimatedSalesValue",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
