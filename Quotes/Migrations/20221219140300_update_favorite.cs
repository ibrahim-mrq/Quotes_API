using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quotes.Migrations
{
    /// <inheritdoc />
    public partial class updatefavorite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuoteResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteResponse", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_QuoteId",
                table: "Favorites",
                column: "QuoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_QuoteResponse_QuoteId",
                table: "Favorites",
                column: "QuoteId",
                principalTable: "QuoteResponse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_QuoteResponse_QuoteId",
                table: "Favorites");

            migrationBuilder.DropTable(
                name: "QuoteResponse");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_QuoteId",
                table: "Favorites");
        }
    }
}
