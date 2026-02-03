using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coffer.Migrations
{
    /// <inheritdoc />
    public partial class LedgerAndMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LedgerId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ISONum = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    Decimals = table.Column<int>(type: "INTEGER", nullable: false),
                    ISOName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ledgers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerId = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    DefaultShareType = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultCurrencyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ledgers_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ledgers_Currencies_DefaultCurrencyId",
                        column: x => x.DefaultCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Costs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    PayedOn = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Added = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AddedById = table.Column<string>(type: "TEXT", nullable: true),
                    PayedById = table.Column<string>(type: "TEXT", nullable: true),
                    LedgerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Costs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Costs_AspNetUsers_AddedById",
                        column: x => x.AddedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Costs_AspNetUsers_PayedById",
                        column: x => x.PayedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Costs_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Costs_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "IOUs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OwedByUserId = table.Column<string>(type: "TEXT", nullable: true),
                    OwedToUserId = table.Column<string>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    SettledAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Settled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LedgerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IOUs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IOUs_AspNetUsers_OwedByUserId",
                        column: x => x.OwedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IOUs_AspNetUsers_OwedToUserId",
                        column: x => x.OwedToUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IOUs_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IOUs_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Modifier = table.Column<decimal>(type: "TEXT", nullable: false),
                    ShareCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Split = table.Column<decimal>(type: "TEXT", nullable: false),
                    SplitCurrencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    LedgerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shares_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shares_Currencies_ShareCurrencyId",
                        column: x => x.ShareCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shares_Currencies_SplitCurrencyId",
                        column: x => x.SplitCurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shares_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Stakes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    HolderId = table.Column<string>(type: "TEXT", nullable: true),
                    AmountStaked = table.Column<decimal>(type: "TEXT", nullable: false),
                    LedgerId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stakes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stakes_AspNetUsers_HolderId",
                        column: x => x.HolderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stakes_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledgers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ShareId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Shares_ShareId",
                        column: x => x.ShareId,
                        principalTable: "Shares",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CostTag",
                columns: table => new
                {
                    CostsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TagsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostTag", x => new { x.CostsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_CostTag_Costs_CostsId",
                        column: x => x.CostsId,
                        principalTable: "Costs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CostTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_LedgerId",
                table: "AspNetUsers",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_AddedById",
                table: "Costs",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_CurrencyId",
                table: "Costs",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_LedgerId",
                table: "Costs",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Costs_PayedById",
                table: "Costs",
                column: "PayedById");

            migrationBuilder.CreateIndex(
                name: "IX_CostTag_TagsId",
                table: "CostTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_IOUs_CurrencyId",
                table: "IOUs",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_IOUs_LedgerId",
                table: "IOUs",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_IOUs_OwedByUserId",
                table: "IOUs",
                column: "OwedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IOUs_OwedToUserId",
                table: "IOUs",
                column: "OwedToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_DefaultCurrencyId",
                table: "Ledgers",
                column: "DefaultCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_OwnerId",
                table: "Ledgers",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_LedgerId",
                table: "Shares",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_ShareCurrencyId",
                table: "Shares",
                column: "ShareCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_SplitCurrencyId",
                table: "Shares",
                column: "SplitCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Shares_UserId",
                table: "Shares",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Stakes_HolderId",
                table: "Stakes",
                column: "HolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Stakes_LedgerId",
                table: "Stakes",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ShareId",
                table: "Tags",
                column: "ShareId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Ledgers_LedgerId",
                table: "AspNetUsers",
                column: "LedgerId",
                principalTable: "Ledgers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Ledgers_LedgerId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CostTag");

            migrationBuilder.DropTable(
                name: "IOUs");

            migrationBuilder.DropTable(
                name: "Stakes");

            migrationBuilder.DropTable(
                name: "Costs");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Shares");

            migrationBuilder.DropTable(
                name: "Ledgers");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_LedgerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "AspNetUsers");
        }
    }
}
