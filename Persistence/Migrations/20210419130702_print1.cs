using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Data.Migrations
{
    public partial class print1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "confirmedBy",
                table: "Projects",
                newName: "ConfirmedBy");

            migrationBuilder.RenameColumn(
                name: "acceptedDate",
                table: "Projects",
                newName: "AcceptedDate");

            migrationBuilder.RenameColumn(
                name: "createdDate",
                table: "Calculations",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "Calculations",
                newName: "CreatedBy");

            migrationBuilder.RenameColumn(
                name: "confirmedDate",
                table: "Calculations",
                newName: "ConfirmedDate");

            migrationBuilder.RenameColumn(
                name: "confirmedBy",
                table: "Calculations",
                newName: "ConfirmedBy");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProjectId",
                table: "Categories",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Projects_ProjectId",
                table: "Categories",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Projects_ProjectId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ProjectId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "ConfirmedBy",
                table: "Projects",
                newName: "confirmedBy");

            migrationBuilder.RenameColumn(
                name: "AcceptedDate",
                table: "Projects",
                newName: "acceptedDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Calculations",
                newName: "createdDate");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Calculations",
                newName: "createdBy");

            migrationBuilder.RenameColumn(
                name: "ConfirmedDate",
                table: "Calculations",
                newName: "confirmedDate");

            migrationBuilder.RenameColumn(
                name: "ConfirmedBy",
                table: "Calculations",
                newName: "confirmedBy");
        }
    }
}
