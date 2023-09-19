using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DDD.Infra.Data.Migrations
{
    public partial class Create_Table_Community : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CommunityId",
                table: "Promotions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CommunityId",
                table: "JoinRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CommunityId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_CommunityId",
                table: "Promotions",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_CommunityId",
                table: "JoinRequests",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CommunityId",
                table: "AspNetUsers",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Communities_CommunityId",
                table: "AspNetUsers",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JoinRequests_Communities_CommunityId",
                table: "JoinRequests",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Communities_CommunityId",
                table: "Promotions",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Communities_CommunityId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_JoinRequests_Communities_CommunityId",
                table: "JoinRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Communities_CommunityId",
                table: "Promotions");

            migrationBuilder.DropTable(
                name: "Communities");

            migrationBuilder.DropIndex(
                name: "IX_Promotions_CommunityId",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_JoinRequests_CommunityId",
                table: "JoinRequests");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CommunityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "Promotions");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "JoinRequests");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "AspNetUsers");
        }
    }
}
