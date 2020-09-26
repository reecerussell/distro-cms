using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Migrations.Migrations
{
    public partial class SupportedCultures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DictionaryItems_Key_CultureName",
                table: "DictionaryItems");

            migrationBuilder.DropColumn(
                name: "CultureName",
                table: "DictionaryItems");

            migrationBuilder.AddColumn<string>(
                name: "CultureId",
                table: "DictionaryItems",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SupportedCultures",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 36, nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(maxLength: 14, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 255, nullable: false),
                    IsDefault = table.Column<bool>(nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportedCultures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryItems_CultureId",
                table: "DictionaryItems",
                column: "CultureId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryItems_Key_CultureId",
                table: "DictionaryItems",
                columns: new[] { "Key", "CultureId" });

            migrationBuilder.CreateIndex(
                name: "IX_SupportedCultures_Name",
                table: "SupportedCultures",
                column: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_DictionaryItems_SupportedCultures_CultureId",
                table: "DictionaryItems",
                column: "CultureId",
                principalTable: "SupportedCultures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DictionaryItems_SupportedCultures_CultureId",
                table: "DictionaryItems");

            migrationBuilder.DropTable(
                name: "SupportedCultures");

            migrationBuilder.DropIndex(
                name: "IX_DictionaryItems_CultureId",
                table: "DictionaryItems");

            migrationBuilder.DropIndex(
                name: "IX_DictionaryItems_Key_CultureId",
                table: "DictionaryItems");

            migrationBuilder.DropColumn(
                name: "CultureId",
                table: "DictionaryItems");

            migrationBuilder.AddColumn<string>(
                name: "CultureName",
                table: "DictionaryItems",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryItems_Key_CultureName",
                table: "DictionaryItems",
                columns: new[] { "Key", "CultureName" });
        }
    }
}
