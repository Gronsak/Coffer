using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffer.Migrations
{
    /// <inheritdoc />
    public partial class AllTagsProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LedgerId",
                table: "Tags",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_LedgerId",
                table: "Tags",
                column: "LedgerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Ledgers_LedgerId",
                table: "Tags",
                column: "LedgerId",
                principalTable: "Ledgers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Ledgers_LedgerId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_LedgerId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "Tags");
        }
    }
}
