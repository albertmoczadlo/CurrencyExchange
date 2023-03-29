using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JediApp.Services.Migrations
{
    public partial class AddRelationExchOffExchBoardCurrencyMoneyOnStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExchangeOfficeBoardId",
                table: "ExchangeOffices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ExchangeOfficeBoardId",
                table: "Currencys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ExchangeOfficeBoards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExchangeOfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeOfficeBoards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeOfficeBoards_ExchangeOffices_ExchangeOfficeId",
                        column: x => x.ExchangeOfficeId,
                        principalTable: "ExchangeOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoneyOnStocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExchangeOfficeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoneyOnStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoneyOnStocks_ExchangeOffices_ExchangeOfficeId",
                        column: x => x.ExchangeOfficeId,
                        principalTable: "ExchangeOffices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Currencys_ExchangeOfficeBoardId",
                table: "Currencys",
                column: "ExchangeOfficeBoardId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOfficeBoards_ExchangeOfficeId",
                table: "ExchangeOfficeBoards",
                column: "ExchangeOfficeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoneyOnStocks_ExchangeOfficeId",
                table: "MoneyOnStocks",
                column: "ExchangeOfficeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Currencys_ExchangeOfficeBoards_ExchangeOfficeBoardId",
                table: "Currencys",
                column: "ExchangeOfficeBoardId",
                principalTable: "ExchangeOfficeBoards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Currencys_ExchangeOfficeBoards_ExchangeOfficeBoardId",
                table: "Currencys");

            migrationBuilder.DropTable(
                name: "ExchangeOfficeBoards");

            migrationBuilder.DropTable(
                name: "MoneyOnStocks");

            migrationBuilder.DropIndex(
                name: "IX_Currencys_ExchangeOfficeBoardId",
                table: "Currencys");

            migrationBuilder.DropColumn(
                name: "ExchangeOfficeBoardId",
                table: "ExchangeOffices");

            migrationBuilder.DropColumn(
                name: "ExchangeOfficeBoardId",
                table: "Currencys");
        }
    }
}
