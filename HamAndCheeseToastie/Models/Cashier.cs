using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("cashiers")] // Matches the table name in the database
    public class Cashier
    {
        [Key]
        [Column("id")] // The primary key column in lowercase
        public int Id { get; set; } // Primary key of the table

        [Column("cashier_id")] // A unique cashier identifier
        [Required] // Ensures the value cannot be null
        public int CashierId { get; set; }

        [Column("first_name")] // Maps to the first_name column
        [Required] // Ensures the value cannot be null
        [MaxLength(100)] // Optional: Limits string length in the database
        public string FirstName { get; set; }

        [Column("last_name")] // Maps to the last_name column
        [Required] // Ensures the value cannot be null
        [MaxLength(100)] // Optional: Limits string length in the database
        public string LastName { get; set; }

        [Column("employee_code")] // Maps to the employee_code column
        [Required] // Ensures the value cannot be null
        [MaxLength(50)] // Optional: Limits string length in the database
        public string EmployeeCode { get; set; }

        [Column("role_id")] // Maps to the role_id column
        [Required] // Ensures the value cannot be null
        public int RoleId { get; set; } // Role identifier for the cashier
    }
}
