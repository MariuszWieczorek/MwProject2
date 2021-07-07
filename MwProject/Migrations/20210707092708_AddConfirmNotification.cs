using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddConfirmNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedDate",
                table: "Notifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductGroupId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ProductGroupId",
                table: "Categories",
                column: "ProductGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_ProductGroups_ProductGroupId",
                table: "Categories",
                column: "ProductGroupId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_ProductGroups_ProductGroupId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ProductGroupId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ConfirmedDate",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ProductGroupId",
                table: "Categories");
        }
    }
}
