using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class _AddCountryFieldtoTableAuthorJoinRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "JoinRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "Authors",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_CountryId",
                table: "JoinRequests",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CountryId",
                table: "Authors",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Countries_CountryId",
                table: "Authors",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Countries_CountryId",
                table: "JoinRequests",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Countries_CountryId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Countries_CountryId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_CountryId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_Authors_CountryId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Authors");
        }
    }
}
