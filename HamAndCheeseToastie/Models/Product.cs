using HamAndCheeseToastie.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("products")] // Set table name to lowercase for PostgreSQL
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;

        [Column("Category_id")]
        public int Category_id { get; set; }
        public virtual Category Category { get; set; } // Navigation Property
        public int CurrentStockLevel { get; set; }
        public int MinimumStockLevel { get; set; }
        public decimal Price { get; set; }
        public decimal WholesalePrice { get; set; }
        public string EAN13Barcode { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }
}
