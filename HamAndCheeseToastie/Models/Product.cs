using HamAndCheeseToastie.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")] // Map to lowercase 'id' column in PostgreSQL
        public int ID { get; set; }

        [Column("name")] // Map to lowercase 'name' column in PostgreSQL
        public string Name { get; set; } = string.Empty;

        [Column("brandname")] // Map to lowercase 'brand_name' column in PostgreSQL
        public string BrandName { get; set; } = string.Empty;

        [Column("weight")] // Map to lowercase 'weight' column in PostgreSQL
        public string Weight { get; set; } = string.Empty;

        [Column("category_id")] // Map to lowercase 'category_id' column in PostgreSQL
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Column("currentstocklevel")] // Map to lowercase 'current_stock_level' column in PostgreSQL
        public int CurrentStockLevel { get; set; }

        [Column("minimumstocklevel")] // Map to lowercase 'minimum_stock_level' column in PostgreSQL
        public int MinimumStockLevel { get; set; }

        [Column("price")] // Map to lowercase 'price' column in PostgreSQL
        public decimal Price { get; set; }

        [Column("wholesaleprice")] // Map to lowercase 'wholesale_price' column in PostgreSQL
        public decimal WholesalePrice { get; set; }

        [Column("ean13barcode")] // Map to lowercase 'ean13_barcode' column in PostgreSQL
        public string EAN13Barcode { get; set; } = string.Empty;

        [Column("imagepath")] // Map to lowercase 'image_path' column in PostgreSQL
        public string ImagePath { get; set; }
    }
}

