using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class transactionFixLast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSimplified");

            migrationBuilder.AddColumn<Guid>(
                name: "TransactionId",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_TransactionId",
                table: "Products",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Transactions_TransactionId",
                table: "Products",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Transactions_TransactionId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TransactionId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductSimplified",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSimplified", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSimplified_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSimplified_TransactionId",
                table: "ProductSimplified",
                column: "TransactionId");
        }
    }
}
