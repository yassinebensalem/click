using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class changecolumnUserIdintablePromoUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoUsers_AspNetUsers_UserIdId",
                table: "PromoUsers");

            migrationBuilder.DropIndex(
                name: "IX_PromoUsers_UserIdId",
                table: "PromoUsers");

            migrationBuilder.DropColumn(
                name: "UserIdId",
                table: "PromoUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PromoUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoUsers_UserId",
                table: "PromoUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoUsers_AspNetUsers_UserId",
                table: "PromoUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoUsers_AspNetUsers_UserId",
                table: "PromoUsers");

            migrationBuilder.DropIndex(
                name: "IX_PromoUsers_UserId",
                table: "PromoUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PromoUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIdId",
                table: "PromoUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoUsers_UserIdId",
                table: "PromoUsers",
                column: "UserIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoUsers_AspNetUsers_UserIdId",
                table: "PromoUsers",
                column: "UserIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
