using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffer.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyShares : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Currencies_SplitCurrencyISONum",
                table: "Shares");

            migrationBuilder.DropIndex(
                name: "IX_Shares_SplitCurrencyISONum",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "Modifier",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "SplitCurrencyISONum",
                table: "Shares");

            migrationBuilder.RenameColumn(
                name: "Split",
                table: "Shares",
                newName: "Size");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Shares",
                newName: "Split");

            migrationBuilder.AddColumn<decimal>(
                name: "Modifier",
                table: "Shares",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SplitCurrencyISONum",
                table: "Shares",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shares_SplitCurrencyISONum",
                table: "Shares",
                column: "SplitCurrencyISONum");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Currencies_SplitCurrencyISONum",
                table: "Shares",
                column: "SplitCurrencyISONum",
                principalTable: "Currencies",
                principalColumn: "ISONum",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
