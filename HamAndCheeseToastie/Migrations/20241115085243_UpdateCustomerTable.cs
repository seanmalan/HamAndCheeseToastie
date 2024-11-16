using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HamAndCheeseToastie.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Users",
                newName: "updatedat");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "Users",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "passwordhash");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Customer",
                newName: "phonenumber");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Customer",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "IsLoyaltyMember",
                table: "Customer",
                newName: "isloyaltymember");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Customer",
                newName: "firstname");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Customer",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Barcode",
                table: "Customer",
                newName: "barcode");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Customer",
                newName: "customerid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "updatedat",
                table: "Users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "Users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "passwordhash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "createdat",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "phonenumber",
                table: "Customer",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "Customer",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "isloyaltymember",
                table: "Customer",
                newName: "IsLoyaltyMember");

            migrationBuilder.RenameColumn(
                name: "firstname",
                table: "Customer",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Customer",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "barcode",
                table: "Customer",
                newName: "Barcode");

            migrationBuilder.RenameColumn(
                name: "customerid",
                table: "Customer",
                newName: "CustomerId");
        }
    }
}
