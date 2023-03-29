using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JediApp.Services.Migrations
{
    public partial class ChangeRelationExchangeOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeOfficeBoards_ExchangeOffices_ExchangeOfficeId",
                table: "ExchangeOfficeBoards");

            migrationBuilder.DropColumn(
                name: "ExchangeOfficeBoardId",
                table: "ExchangeOffices");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeOfficeBoards_ExchangeOffices_ExchangeOfficeId",
                table: "ExchangeOfficeBoards",
                column: "ExchangeOfficeId",
                principalTable: "ExchangeOffices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeOfficeBoards_ExchangeOffices_ExchangeOfficeId",
                table: "ExchangeOfficeBoards");

            migrationBuilder.AddColumn<Guid>(
                name: "ExchangeOfficeBoardId",
                table: "ExchangeOffices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeOfficeBoards_ExchangeOffices_ExchangeOfficeId",
                table: "ExchangeOfficeBoards",
                column: "ExchangeOfficeId",
                principalTable: "ExchangeOffices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
