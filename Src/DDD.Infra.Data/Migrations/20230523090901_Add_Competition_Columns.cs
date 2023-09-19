using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class Add_Competition_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DesiredBooks",
                table: "JoinRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VoucherNumber",
                table: "JoinRequests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VoucherValue",
                table: "JoinRequests",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DesiredBooks",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "VoucherNumber",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "VoucherValue",
                table: "JoinRequests");
        }
    }
}
