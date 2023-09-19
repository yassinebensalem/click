using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class updatetablePromotion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Promotions",
                newName: "Percentage");

            migrationBuilder.RenameColumn(
                name: "StartPublicationDate",
                table: "Promotions",
                newName: "StartDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Promotions",
                newName: "StartPublicationDate");

            migrationBuilder.RenameColumn(
                name: "Percentage",
                table: "Promotions",
                newName: "Value");
        }
    }
}
