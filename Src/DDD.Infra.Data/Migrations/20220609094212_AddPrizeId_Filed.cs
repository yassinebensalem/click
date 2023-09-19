using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class AddPrizeId_Filed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Countries__CountryId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_Countries_CountryId",
                table: "Prizes");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Prizes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "_CountryId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "FK_Books_Countries__CountryId",
                table: "Books",
                column: "_CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Prizes_PrizedId",
                table: "Books",
                column: "PrizedId",
                principalTable: "Prizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Prizes_Countries_CountryId",
                table: "Prizes",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Countries__CountryId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Prizes_PrizedId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Prizes_Countries_CountryId",
                table: "Prizes");

            migrationBuilder.DropIndex(
                name: "IX_Books_PrizedId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "PrizedId",
                table: "Books");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Prizes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "_CountryId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Countries__CountryId",
                table: "Books",
                column: "_CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Prizes_Countries_CountryId",
                table: "Prizes",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
