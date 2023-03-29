using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JediApp.Services.Migrations
{
    public partial class addCurrencyDictionary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyDictionaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyDictionaries", x => x.Id);
                });

            migrationBuilder.Sql(@"INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Australian dollar', N'AUD', N'Australia');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Canadian dollar', N'CAD', N'Canada');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Swiss franc', N'CHF', N'Switzerland');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Czech koruna', N'CZK', N'Czech Republic');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Danish krone', N'DKK', N'Denmark');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Euro', N'EUR', N'European Union (EU)');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'British pound', N'GBP', N'United Kingdom');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Hungarian forint', N'HUF', N'Hungary');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Japanese yen', N'JPY', N'Japan');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Norwegian krone', N'NOK', N'Norway');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Swedish krona', N'SEK', N'Sweden');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'United States dollar', N'USD', N'United States');
                INSERT [dbo].[CurrencyDictionaries] ( [Name], [ShortName], [Country]) VALUES (N'Polish zloty', N'PLN', N'Poland');
                
            ");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyDictionaries");
        }
    }
}
