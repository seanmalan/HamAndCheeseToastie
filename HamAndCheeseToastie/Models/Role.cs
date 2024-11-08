using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("roles")] // Set table name to lowercase for PostgreSQL compatibility
    public class Role
    {
        [Key]
        [Column("id")] // Set column name to lowercase
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
