using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("cashiers")] // Specify table name in lowercase to avoid issues
    public class Cashier
    {
        [Key]
        [Column("id")] // Specify column name in lowercase
        public int Id { get; set; }

        [Column("cashier_id")] // Naming convention to avoid case-sensitivity issues
        public int CashierId { get; set; }               // Unique ID for the cashier

        [Column("first_name")] // Naming convention
        public string FirstName { get; set; }            // Cashier's first name

        [Column("last_name")] // Naming convention
        public string LastName { get; set; }             // Cashier's last name

        [Column("employee_code")] // Naming convention
        public string EmployeeCode { get; set; }         // Cashier's employee code or ID
    }
}
