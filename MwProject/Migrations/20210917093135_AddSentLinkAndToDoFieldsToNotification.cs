using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class AddSentLinkAndToDoFieldsToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Sent",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ToDo",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Sent",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ToDo",
                table: "Notifications");
        }
    }
}
