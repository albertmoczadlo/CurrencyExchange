using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JediApp.Services.Migrations
{
    public partial class RemoveRelationTransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistory_AspNetUsers_UserId1",
                table: "TransactionHistory");

            migrationBuilder.DropIndex(
                name: "IX_TransactionHistory_UserId1",
                table: "TransactionHistory");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TransactionHistory");

            migrationBuilder.DropColumn(
                name: "UserLogin",
                table: "TransactionHistory");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TransactionHistory",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_UserId",
                table: "TransactionHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistory_AspNetUsers_UserId",
                table: "TransactionHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistory_AspNetUsers_UserId",
                table: "TransactionHistory");

            migrationBuilder.DropIndex(
                name: "IX_TransactionHistory_UserId",
                table: "TransactionHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "TransactionHistory",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "TransactionHistory",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserLogin",
                table: "TransactionHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_UserId1",
                table: "TransactionHistory",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistory_AspNetUsers_UserId1",
                table: "TransactionHistory",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
