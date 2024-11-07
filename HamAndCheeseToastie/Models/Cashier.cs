using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    public class Cashier
    {
        [Key]
        public int Id { get; set; }
        public int CashierId { get; set; }               // Unique ID for the cashier
        public string FirstName { get; set; }            // Cashier's first name
        public string LastName { get; set; }             // Cashier's last name
        public string EmployeeCode { get; set; }         // Cashier's employee code or ID
        public int RoleId { get; set; }               // Unique ID for the cashier

    }

}
