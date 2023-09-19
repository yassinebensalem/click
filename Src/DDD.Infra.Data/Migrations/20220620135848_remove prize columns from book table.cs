using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class removeprizecolumnsfrombooktable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Prizes_PrizedId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_PrizedId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PrizedId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Prizes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Prizes");

            migrationBuilder.AddColumn<Guid>(
                name: "PrizedId",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_PrizedId",
                table: "Books",
                column: "PrizedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Prizes_PrizedId",
                table: "Books",
                column: "PrizedId",
                principalTable: "Prizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
