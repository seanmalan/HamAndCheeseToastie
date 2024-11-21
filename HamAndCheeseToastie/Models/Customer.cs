using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("customers")] // Specify table name in lowercase for PostgreSQL compatibility
    public class Customer
    {
        [Key]
        [Column("customerid")] // Map to 'customer_id' column in PostgreSQL
        public int CustomerId { get; set; } // Unique ID for the customer

        [Column("firstname")] // Map to 'first_name' column in PostgreSQL
        public string FirstName { get; set; } // Customer's first name

        [Column("lastname")] // Map to 'last_name' column in PostgreSQL
        public string LastName { get; set; } // Customer's last name

        [Column("email")] // Map to 'email' column in PostgreSQL
        public string? Email { get; set; } // Customer's email (optional)

        [Column("phonenumber")] // Map to 'phone_number' column in PostgreSQL
        public string? PhoneNumber { get; set; } // Customer's phone number (optional)

        [Column("isloyaltymember")] // Map to 'is_loyalty_member' column in PostgreSQL
        public bool IsLoyaltyMember { get; set; } // Is the customer a loyalty program member?

        [Column("barcode")] // Map to 'barcode' column in PostgreSQL
        public string? Barcode { get; set; } // Barcode for loyalty program
    }
}