using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffer.Migrations
{
    /// <inheritdoc />
    public partial class ManyTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Shares_ShareId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ShareId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ShareId",
                table: "Tags");

            migrationBuilder.CreateTable(
                name: "ShareTag",
                columns: table => new
                {
                    SharesId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareTag", x => new { x.SharesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ShareTag_Shares_SharesId",
                        column: x => x.SharesId,
                        principalTable: "Shares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShareTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareTag_TagsId",
                table: "ShareTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShareTag");

            migrationBuilder.AddColumn<Guid>(
                name: "ShareId",
                table: "Tags",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ShareId",
                table: "Tags",
                column: "ShareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Shares_ShareId",
                table: "Tags",
                column: "ShareId",
                principalTable: "Shares",
                principalColumn: "Id");
        }
    }
}
