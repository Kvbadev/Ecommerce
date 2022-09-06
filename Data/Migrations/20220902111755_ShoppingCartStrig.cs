using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class ShoppingCartStrig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ShoppingCart_ShoppingCartRef",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShoppingCartRef",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShoppingCartRef",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "ShoppingCart",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCart_AppUserId",
                table: "ShoppingCart",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_AppUserId",
                table: "ShoppingCart",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_AppUserId",
                table: "ShoppingCart");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCart_AppUserId",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ShoppingCart");

            migrationBuilder.AddColumn<Guid>(
                name: "ShoppingCartRef",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShoppingCartRef",
                table: "AspNetUsers",
                column: "ShoppingCartRef",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ShoppingCart_ShoppingCartRef",
                table: "AspNetUsers",
                column: "ShoppingCartRef",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
