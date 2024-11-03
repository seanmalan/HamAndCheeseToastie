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
        [Column("id")] // Set column name to lowercase
        public int ID { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("brand_name")]
        public string BrandName { get; set; } = string.Empty;

        [Column("weight")]
        public string Weight { get; set; } = string.Empty;

        [Column("category_id")]
        public int Category_id { get; set; }

        [ForeignKey("Category_id")]
        public virtual Category Category { get; set; } // Navigation Property

        [Column("current_stock_level")]
        public int CurrentStockLevel { get; set; }

        [Column("minimum_stock_level")]
        public int MinimumStockLevel { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("wholesale_price")]
        public decimal WholesalePrice { get; set; }

        [Column("ean13_barcode")]
        public string EAN13Barcode { get; set; } = string.Empty;

        [Column("image_path")]
        public string ImagePath { get; set; } = string.Empty;
    }
}
