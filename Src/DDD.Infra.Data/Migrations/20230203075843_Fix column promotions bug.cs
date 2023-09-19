using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class Fixcolumnpromotionsbug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxFreeBooks",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxFreeBooks",
                table: "Promotions");
        }
    }
}
