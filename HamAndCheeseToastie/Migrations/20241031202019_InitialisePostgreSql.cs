using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    /// <inheritdoc />
    public partial class InitialisePostgreSql : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Products_ProductId",
                table: "TransactionItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Transaction_TransactionId",
                table: "TransactionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionItem",
                table: "TransactionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cashier",
                table: "Cashier");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "roles");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "products");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "categories");

            migrationBuilder.RenameTable(
                name: "TransactionItem",
                newName: "transaction_items");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "transactions");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "customers");

            migrationBuilder.RenameTable(
                name: "Cashier",
                newName: "cashiers");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "users",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "EmailConfirmed",
                table: "users",
                newName: "email_confirmed");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "roles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "products",
                newName: "weight");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Category_id",
                table: "products",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "products",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "WholesalePrice",
                table: "products",
                newName: "wholesale_price");

            migrationBuilder.RenameColumn(
                name: "MinimumStockLevel",
                table: "products",
                newName: "minimum_stock_level");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "products",
                newName: "image_path");

            migrationBuilder.RenameColumn(
                name: "EAN13Barcode",
                table: "products",
                newName: "ean13_barcode");

            migrationBuilder.RenameColumn(
                name: "CurrentStockLevel",
                table: "products",
                newName: "current_stock_level");

            migrationBuilder.RenameColumn(
                name: "BrandName",
                table: "products",
                newName: "brand_name");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Category_id",
                table: "products",
                newName: "IX_products_category_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "transaction_items",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "transaction_items",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "transaction_items",
                newName: "unit_price");

            migrationBuilder.RenameColumn(
                name: "TransactionItemId",
                table: "transaction_items",
                newName: "transaction_item_id");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "transaction_items",
                newName: "transaction_id");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "transaction_items",
                newName: "total_price");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "transaction_items",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_TransactionId",
                table: "transaction_items",
                newName: "IX_transaction_items_transaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionItem_ProductId",
                table: "transaction_items",
                newName: "IX_transaction_items_product_id");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "transactions",
                newName: "discount");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "transactions",
                newName: "transaction_date");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "transactions",
                newName: "total_amount");

            migrationBuilder.RenameColumn(
                name: "TaxAmount",
                table: "transactions",
                newName: "tax_amount");

            migrationBuilder.RenameColumn(
                name: "PaymentMethod",
                table: "transactions",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "transactions",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "CashierId",
                table: "transactions",
                newName: "cashier_id");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "transactions",
                newName: "transaction_id");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_CustomerId",
                table: "transactions",
                newName: "IX_transactions_customer_id");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_CashierId",
                table: "transactions",
                newName: "IX_transactions_cashier_id");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "customers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "customers",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "customers",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "IsLoyaltyMember",
                table: "customers",
                newName: "is_loyalty_member");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "customers",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "customers",
                newName: "customer_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "cashiers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "cashiers",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "cashiers",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "EmployeeCode",
                table: "cashiers",
                newName: "employee_code");

            migrationBuilder.RenameColumn(
                name: "CashierId",
                table: "cashiers",
                newName: "cashier_id");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_roles",
                table: "roles",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_products",
                table: "products",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transaction_items",
                table: "transaction_items",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                table: "transactions",
                column: "transaction_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_customers",
                table: "customers",
                column: "customer_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cashiers",
                table: "cashiers",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_category_id",
                table: "products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_items_products_product_id",
                table: "transaction_items",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transaction_items_transactions_transaction_id",
                table: "transaction_items",
                column: "transaction_id",
                principalTable: "transactions",
                principalColumn: "transaction_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_cashiers_cashier_id",
                table: "transactions",
                column: "cashier_id",
                principalTable: "cashiers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_customers_customer_id",
                table: "transactions",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_category_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_transaction_items_products_product_id",
                table: "transaction_items");

            migrationBuilder.DropForeignKey(
                name: "FK_transaction_items_transactions_transaction_id",
                table: "transaction_items");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_cashiers_cashier_id",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_customers_customer_id",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_roles",
                table: "roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_products",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transaction_items",
                table: "transaction_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_customers",
                table: "customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cashiers",
                table: "cashiers");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "roles",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "products",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "transactions",
                newName: "Transaction");

            migrationBuilder.RenameTable(
                name: "transaction_items",
                newName: "TransactionItem");

            migrationBuilder.RenameTable(
                name: "customers",
                newName: "Customer");

            migrationBuilder.RenameTable(
                name: "cashiers",
                newName: "Cashier");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "email_confirmed",
                table: "Users",
                newName: "EmailConfirmed");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Roles",
                newName: "Id");

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
                name: "category_id",
                table: "Products",
                newName: "Category_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Products",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "wholesale_price",
                table: "Products",
                newName: "WholesalePrice");

            migrationBuilder.RenameColumn(
                name: "minimum_stock_level",
                table: "Products",
                newName: "MinimumStockLevel");

            migrationBuilder.RenameColumn(
                name: "image_path",
                table: "Products",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "ean13_barcode",
                table: "Products",
                newName: "EAN13Barcode");

            migrationBuilder.RenameColumn(
                name: "current_stock_level",
                table: "Products",
                newName: "CurrentStockLevel");

            migrationBuilder.RenameColumn(
                name: "brand_name",
                table: "Products",
                newName: "BrandName");

            migrationBuilder.RenameIndex(
                name: "IX_products_category_id",
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

            migrationBuilder.RenameColumn(
                name: "discount",
                table: "Transaction",
                newName: "Discount");

            migrationBuilder.RenameColumn(
                name: "transaction_date",
                table: "Transaction",
                newName: "TransactionDate");

            migrationBuilder.RenameColumn(
                name: "total_amount",
                table: "Transaction",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "tax_amount",
                table: "Transaction",
                newName: "TaxAmount");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "Transaction",
                newName: "PaymentMethod");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "Transaction",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "cashier_id",
                table: "Transaction",
                newName: "CashierId");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "Transaction",
                newName: "TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_customer_id",
                table: "Transaction",
                newName: "IX_Transaction_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_cashier_id",
                table: "Transaction",
                newName: "IX_Transaction_CashierId");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "TransactionItem",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "TransactionItem",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "unit_price",
                table: "TransactionItem",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "transaction_item_id",
                table: "TransactionItem",
                newName: "TransactionItemId");

            migrationBuilder.RenameColumn(
                name: "transaction_id",
                table: "TransactionItem",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "total_price",
                table: "TransactionItem",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "TransactionItem",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_transaction_items_transaction_id",
                table: "TransactionItem",
                newName: "IX_TransactionItem_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_transaction_items_product_id",
                table: "TransactionItem",
                newName: "IX_TransactionItem_ProductId");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Customer",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "Customer",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Customer",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "is_loyalty_member",
                table: "Customer",
                newName: "IsLoyaltyMember");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Customer",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "customer_id",
                table: "Customer",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Cashier",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "Cashier",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                table: "Cashier",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "employee_code",
                table: "Cashier",
                newName: "EmployeeCode");

            migrationBuilder.RenameColumn(
                name: "cashier_id",
                table: "Cashier",
                newName: "CashierId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customer",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Customer",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionItem",
                table: "TransactionItem",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cashier",
                table: "Cashier",
                column: "Id");

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Customer_CustomerId",
                table: "Transaction",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Products_ProductId",
                table: "TransactionItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Transaction_TransactionId",
                table: "TransactionItem",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
