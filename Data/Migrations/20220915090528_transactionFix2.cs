using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class transactionFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_BuyerId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "BuyerId",
                table: "Transactions",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_BuyerId",
                table: "Transactions",
                newName: "IX_Transactions_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_AppUserId",
                table: "Transactions",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_AppUserId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Transactions",
                newName: "BuyerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AppUserId",
                table: "Transactions",
                newName: "IX_Transactions_BuyerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_BuyerId",
                table: "Transactions",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
