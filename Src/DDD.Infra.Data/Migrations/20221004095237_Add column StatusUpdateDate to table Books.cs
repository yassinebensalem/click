using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class AddcolumnStatusUpdateDatetotableBooks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StatusUpdateDate",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusUpdateDate",
                table: "Books");
        }
    }
}
