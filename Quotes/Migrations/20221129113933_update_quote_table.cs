using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quotes.Migrations
{
    /// <inheritdoc />
    public partial class updatequotetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "Quotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Quotes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Quotes");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Quotes");
        }
    }
}
