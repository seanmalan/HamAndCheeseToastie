using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsProductTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_Category_id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Cashier_CashierId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Customer_CustomerId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "Transaction",
                newName: "transactiondate");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Transaction",
                newName: "totalamount");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "Transaction",
                newName: "taxamount");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "Transaction",
                newName: "paymentmethod");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Transaction",
                newName: "discount");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Transaction",
                newName: "customerid");

            migrationBuilder.RenameColumn(
                name: "CashierId",
                table: "Transaction",
                newName: "cashierid");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Transaction",
                newName: "transactionid");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_CustomerId",
                table: "Transaction",
                newName: "IX_Transaction_customerid");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_CashierId",
                table: "Transaction",
                newName: "IX_Transaction_cashierid");

            migrationBuilder.RenameColumn(
                name: "WholesalePrice",
                table: "Products",
                newName: "wholesaleprice");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Products",
                newName: "weight");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "MinimumStockLevel",
                table: "Products",
                newName: "minimumstocklevel");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Products",
                newName: "imagepath");

            migrationBuilder.RenameColumn(
                name: "EAN13Barcode",
                table: "Products",
                newName: "ean13barcode");

            migrationBuilder.RenameColumn(
                name: "CurrentStockLevel",
                table: "Products",
                newName: "currentstocklevel");

            migrationBuilder.RenameColumn(
                name: "Category_id",
                table: "Products",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "BrandName",
                table: "Products",
                newName: "brandname");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Products",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Category_id",
                table: "Products",
                newName: "IX_Products_category_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Categories",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_category_id",
                table: "Products",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Cashier_cashierid",
                table: "Transaction",
                column: "cashierid",
                principalTable: "Cashier",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Customer_customerid",
                table: "Transaction",
                column: "customerid",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_category_id",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Cashier_cashierid",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Customer_customerid",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "transactiondate",
                table: "Transaction",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "totalamount",
                table: "Transaction",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "taxamount",
                table: "Transaction",
                newName: "TaxAmount");

            migrationBuilder.RenameColumn(
                name: "paymentmethod",
                table: "Transaction",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "discount",
                table: "Transaction",
                newName: "Discount");

            migrationBuilder.RenameColumn(
                name: "customerid",
                table: "Transaction",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "cashierid",
                table: "Transaction",
                newName: "CashierId");

            migrationBuilder.RenameColumn(
                name: "transactionid",
                table: "Transaction",
                newName: "TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_customerid",
                table: "Transaction",
                newName: "IX_Transaction_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_cashierid",
                table: "Transaction",
                newName: "IX_Transaction_CashierId");

            migrationBuilder.RenameColumn(
                name: "wholesaleprice",
                table: "Products",
                newName: "WholesalePrice");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "Products",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "minimumstocklevel",
                table: "Products",
                newName: "MinimumStockLevel");

            migrationBuilder.RenameColumn(
                name: "imagepath",
                table: "Products",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "ean13barcode",
                table: "Products",
                newName: "EAN13Barcode");

            migrationBuilder.RenameColumn(
                name: "currentstocklevel",
                table: "Products",
                newName: "CurrentStockLevel");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Products",
                newName: "Category_id");

            migrationBuilder.RenameColumn(
                name: "brandname",
                table: "Products",
                newName: "BrandName");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Products",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_category_id",
                table: "Products",
                newName: "IX_Products_Category_id");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_Category_id",
                table: "Products",
                column: "Category_id",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Cashier_CashierId",
                table: "Transaction",
                column: "CashierId",
                principalTable: "Cashier",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Customer_CustomerId",
                table: "Transaction",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
