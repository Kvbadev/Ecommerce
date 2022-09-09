using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class cartDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_ShoppingCart_ShoppingCartId",
                table: "CartProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_AppUserId",
                table: "ShoppingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCart",
                table: "ShoppingCart");

            migrationBuilder.RenameTable(
                name: "ShoppingCart",
                newName: "ShoppingCarts");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCart_AppUserId",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_ShoppingCarts_ShoppingCartId",
                table: "CartProducts",
                column: "ShoppingCartId",
                principalTable: "ShoppingCarts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_AppUserId",
                table: "ShoppingCarts",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartProducts_ShoppingCarts_ShoppingCartId",
                table: "CartProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_AspNetUsers_AppUserId",
                table: "ShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts");

            migrationBuilder.RenameTable(
                name: "ShoppingCarts",
                newName: "ShoppingCart");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_AppUserId",
                table: "ShoppingCart",
                newName: "IX_ShoppingCart_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCart",
                table: "ShoppingCart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartProducts_ShoppingCart_ShoppingCartId",
                table: "CartProducts",
                column: "ShoppingCartId",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCart_AspNetUsers_AppUserId",
                table: "ShoppingCart",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
