using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HamAndCheeseToastie.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int CurrentStockLevel { get; set; }
        public int MinimumStockLevel { get; set; }
        public decimal Price { get; set; }
        public decimal WholesalePrice { get; set; }
        public string EAN13Barcode { get; set; } = string.Empty;
        public string ImagePath { get; set; }

    }
}
