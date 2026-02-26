using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffer.Migrations
{
    /// <inheritdoc />
    public partial class AddAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Currencies_ShareCurrencyISONum",
                table: "Shares");

            migrationBuilder.RenameColumn(
                name: "ShareCurrencyISONum",
                table: "Shares",
                newName: "CurrencyISONum");

            migrationBuilder.RenameIndex(
                name: "IX_Shares_ShareCurrencyISONum",
                table: "Shares",
                newName: "IX_Shares_CurrencyISONum");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Shares",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Currencies_CurrencyISONum",
                table: "Shares",
                column: "CurrencyISONum",
                principalTable: "Currencies",
                principalColumn: "ISONum",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Currencies_CurrencyISONum",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Shares");

            migrationBuilder.RenameColumn(
                name: "CurrencyISONum",
                table: "Shares",
                newName: "ShareCurrencyISONum");

            migrationBuilder.RenameIndex(
                name: "IX_Shares_CurrencyISONum",
                table: "Shares",
                newName: "IX_Shares_ShareCurrencyISONum");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Currencies_ShareCurrencyISONum",
                table: "Shares",
                column: "ShareCurrencyISONum",
                principalTable: "Currencies",
                principalColumn: "ISONum",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
