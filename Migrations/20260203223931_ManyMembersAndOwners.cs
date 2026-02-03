using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffer.Migrations
{
    /// <inheritdoc />
    public partial class ManyMembersAndOwners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ledgers_LedgerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_AspNetUsers_OwnerId",
                table: "Ledgers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LedgerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Ledgers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AppUserLedger",
                columns: table => new
                {
                    MemberOfId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MembersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLedger", x => new { x.MemberOfId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_AppUserLedger_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserLedger_Ledgers_MemberOfId",
                        column: x => x.MemberOfId,
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserLedger_MembersId",
                table: "AppUserLedger",
                column: "MembersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_AspNetUsers_OwnerId",
                table: "Ledgers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ledgers_AspNetUsers_OwnerId",
                table: "Ledgers");

            migrationBuilder.DropTable(
                name: "AppUserLedger");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "Ledgers",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<Guid>(
                name: "LedgerId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LedgerId",
                table: "AspNetUsers",
                column: "LedgerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ledgers_LedgerId",
                table: "AspNetUsers",
                column: "LedgerId",
                principalTable: "Ledgers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ledgers_AspNetUsers_OwnerId",
                table: "Ledgers",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
