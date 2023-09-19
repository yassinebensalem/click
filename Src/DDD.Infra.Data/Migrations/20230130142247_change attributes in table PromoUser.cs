using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class changeattributesintablePromoUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoUsers_Prizes_PrizeId",
                table: "PromoUsers");

            migrationBuilder.RenameColumn(
                name: "PrizeId",
                table: "PromoUsers",
                newName: "PromotionId");

            migrationBuilder.RenameIndex(
                name: "IX_PromoUsers_PrizeId",
                table: "PromoUsers",
                newName: "IX_PromoUsers_PromotionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoUsers_Promotions_PromotionId",
                table: "PromoUsers",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoUsers_Promotions_PromotionId",
                table: "PromoUsers");

            migrationBuilder.RenameColumn(
                name: "PromotionId",
                table: "PromoUsers",
                newName: "PrizeId");

            migrationBuilder.RenameIndex(
                name: "IX_PromoUsers_PromotionId",
                table: "PromoUsers",
                newName: "IX_PromoUsers_PrizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoUsers_Prizes_PrizeId",
                table: "PromoUsers",
                column: "PrizeId",
                principalTable: "Prizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
