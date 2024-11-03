using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("customers")] // Specify table name in lowercase
    public class Customer
    {
        [Key]
        [Column("customer_id")] // Specify column name in lowercase
        public int CustomerId { get; set; }               // Unique ID for the customer

        [Column("first_name")]
        public string FirstName { get; set; }             // Customer's first name

        [Column("last_name")]
        public string LastName { get; set; }              // Customer's last name

        [Column("email")]
        public string? Email { get; set; }                // Customer's email (optional)

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }          // Customer's phone number (optional)

        [Column("is_loyalty_member")]
        public bool IsLoyaltyMember { get; set; }         // Is the customer a loyalty program member?
    }
}
