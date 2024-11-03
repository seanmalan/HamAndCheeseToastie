using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("categories")] // Specify table name in lowercase
    public class Category
    {
        [Key]
        [Column("id")] // Specify column name in lowercase
        public int Id { get; set; }

        [Column("name")] // Specify column name in lowercase
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
