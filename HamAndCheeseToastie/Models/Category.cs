using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("categories")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int CategoryId { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; } = string.Empty;

        // Navigation property back to Products (optional)
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
