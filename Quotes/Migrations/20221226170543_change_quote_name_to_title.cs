﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quotes.Migrations
{
    /// <inheritdoc />
    public partial class changequotenametotitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Quotes",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Quotes",
                newName: "Name");
        }
    }
}
