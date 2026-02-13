using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffer.Migrations
{
    /// <inheritdoc />
    public partial class ShareTagAndCostGranularity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShareTag_Shares_SharesId",
                table: "ShareTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareTag_Tags_TagsId",
                table: "ShareTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareTag",
                table: "ShareTag");

            migrationBuilder.DropIndex(
                name: "IX_ShareTag_TagsId",
                table: "ShareTag");

            migrationBuilder.RenameColumn(
                name: "TagsId",
                table: "ShareTag",
                newName: "IncludeTagsId");

            migrationBuilder.RenameColumn(
                name: "SharesId",
                table: "ShareTag",
                newName: "IncludedSharesId");

            migrationBuilder.AddColumn<Guid>(
                name: "SingleCostId",
                table: "Shares",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareTag",
                table: "ShareTag",
                columns: new[] { "IncludeTagsId", "IncludedSharesId" });

            migrationBuilder.CreateTable(
                name: "ShareTag1",
                columns: table => new
                {
                    ExcludeTagsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExcludedSharesId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareTag1", x => new { x.ExcludeTagsId, x.ExcludedSharesId });
                    table.ForeignKey(
                        name: "FK_ShareTag1_Shares_ExcludedSharesId",
                        column: x => x.ExcludedSharesId,
                        principalTable: "Shares",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShareTag1_Tags_ExcludeTagsId",
                        column: x => x.ExcludeTagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShareTag_IncludedSharesId",
                table: "ShareTag",
                column: "IncludedSharesId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_SingleCostId",
                table: "Shares",
                column: "SingleCostId");

            migrationBuilder.CreateIndex(
                name: "IX_ShareTag1_ExcludedSharesId",
                table: "ShareTag1",
                column: "ExcludedSharesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shares_Costs_SingleCostId",
                table: "Shares",
                column: "SingleCostId",
                principalTable: "Costs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareTag_Shares_IncludedSharesId",
                table: "ShareTag",
                column: "IncludedSharesId",
                principalTable: "Shares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareTag_Tags_IncludeTagsId",
                table: "ShareTag",
                column: "IncludeTagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shares_Costs_SingleCostId",
                table: "Shares");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareTag_Shares_IncludedSharesId",
                table: "ShareTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ShareTag_Tags_IncludeTagsId",
                table: "ShareTag");

            migrationBuilder.DropTable(
                name: "ShareTag1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShareTag",
                table: "ShareTag");

            migrationBuilder.DropIndex(
                name: "IX_ShareTag_IncludedSharesId",
                table: "ShareTag");

            migrationBuilder.DropIndex(
                name: "IX_Shares_SingleCostId",
                table: "Shares");

            migrationBuilder.DropColumn(
                name: "SingleCostId",
                table: "Shares");

            migrationBuilder.RenameColumn(
                name: "IncludedSharesId",
                table: "ShareTag",
                newName: "SharesId");

            migrationBuilder.RenameColumn(
                name: "IncludeTagsId",
                table: "ShareTag",
                newName: "TagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShareTag",
                table: "ShareTag",
                columns: new[] { "SharesId", "TagsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShareTag_TagsId",
                table: "ShareTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShareTag_Shares_SharesId",
                table: "ShareTag",
                column: "SharesId",
                principalTable: "Shares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShareTag_Tags_TagsId",
                table: "ShareTag",
                column: "TagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
