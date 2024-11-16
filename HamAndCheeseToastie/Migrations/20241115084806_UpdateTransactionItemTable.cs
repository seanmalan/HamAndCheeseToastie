using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionItemTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Products_ProductId",
                table: "TransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Transaction_TransactionId",
                table: "TransactionItem");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "TransactionItem",
                newName: "unitprice");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "TransactionItem",
                newName: "transactionid");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "TransactionItem",
                newName: "totalprice");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "TransactionItem",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "TransactionItem",
                newName: "productid");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_TransactionId",
                table: "TransactionItem",
                newName: "IX_TransactionItem_transactionid");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_ProductId",
                table: "TransactionItem",
                newName: "IX_TransactionItem_productid");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Products_productid",
                table: "TransactionItem",
                column: "productid",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Transaction_transactionid",
                table: "TransactionItem",
                column: "transactionid",
                principalTable: "Transaction",
                principalColumn: "transactionid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Products_productid",
                table: "TransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Transaction_transactionid",
                table: "TransactionItem");

            migrationBuilder.RenameColumn(
                name: "unitprice",
                table: "TransactionItem",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "transactionid",
                table: "TransactionItem",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "totalprice",
                table: "TransactionItem",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "TransactionItem",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "productid",
                table: "TransactionItem",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_transactionid",
                table: "TransactionItem",
                newName: "IX_TransactionItem_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_productid",
                table: "TransactionItem",
                newName: "IX_TransactionItem_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Products_ProductId",
                table: "TransactionItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Transaction_TransactionId",
                table: "TransactionItem",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "transactionid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
