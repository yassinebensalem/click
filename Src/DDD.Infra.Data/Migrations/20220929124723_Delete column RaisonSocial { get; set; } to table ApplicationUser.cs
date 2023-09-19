using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class DeletecolumnRaisonSocialgetsettotableApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdFiscal",
                table: "JoinRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RaisonSocial",
                table: "JoinRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdFiscal",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "RaisonSocial",
                table: "JoinRequests");
        }
    }
}
