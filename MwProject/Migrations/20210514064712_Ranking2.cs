using Microsoft.EntityFrameworkCore.Migrations;

namespace MwProject.Migrations
{
    public partial class Ranking2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RankingCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbrev = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RankingElements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rangeFrom = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    rangeTo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    RankingCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingElements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankingElements_RankingCategories_RankingCategoryId",
                        column: x => x.RankingCategoryId,
                        principalTable: "RankingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RankingElements_RankingCategoryId",
                table: "RankingElements",
                column: "RankingCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RankingElements");

            migrationBuilder.DropTable(
                name: "RankingCategories");
        }
    }
}
