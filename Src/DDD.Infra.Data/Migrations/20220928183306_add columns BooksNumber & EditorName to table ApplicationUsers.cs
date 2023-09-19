using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class addcolumnsBooksNumberEditorNametotableApplicationUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BooksNumber",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EditorName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BooksNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EditorName",
                table: "AspNetUsers");
        }
    }
}
