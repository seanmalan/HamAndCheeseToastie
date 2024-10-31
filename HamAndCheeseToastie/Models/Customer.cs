using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }               // Unique ID for the customer
        public string FirstName { get; set; }             // Customer's first name
        public string LastName { get; set; }              // Customer's last name
        public string Email { get; set; }                 // Customer's email (optional)
        public string PhoneNumber { get; set; }           // Customer's phone number (optional)
        public bool IsLoyaltyMember { get; set; }         // Is the customer a loyalty program member?
    }

}
