using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JediApp.Services.Migrations
{
    public partial class RemoveUserFromExOfficeRemoveWalletFromCurrencyAddPropExOff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeOffices_AspNetUsers_UserId",
                table: "ExchangeOffices");

            migrationBuilder.DropIndex(
                name: "IX_WalletPositions_CurrencyId",
                table: "WalletPositions");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeOffices_UserId",
                table: "ExchangeOffices");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExchangeOffices");

            migrationBuilder.DropColumn(
                name: "WalletPositionId",
                table: "Currencys");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ExchangeOffices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Markup",
                table: "ExchangeOffices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ExchangeOffices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WalletPositions_CurrencyId",
                table: "WalletPositions",
                column: "CurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WalletPositions_CurrencyId",
                table: "WalletPositions");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "ExchangeOffices");

            migrationBuilder.DropColumn(
                name: "Markup",
                table: "ExchangeOffices");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ExchangeOffices");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ExchangeOffices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "WalletPositionId",
                table: "Currencys",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_WalletPositions_CurrencyId",
                table: "WalletPositions",
                column: "CurrencyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOffices_UserId",
                table: "ExchangeOffices",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeOffices_AspNetUsers_UserId",
                table: "ExchangeOffices",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
