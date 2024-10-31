using HamAndCheeseToastie.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Product
{
    [Key]
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BrandName { get; set; } = string.Empty;
    public string Weight { get; set; } = string.Empty;

    public int Category_id { get; set; }

    [ForeignKey("Category_id")]
    public virtual Category Category { get; set; } // Navigation Property

    public int CurrentStockLevel { get; set; }
    public int MinimumStockLevel { get; set; }
    public decimal Price { get; set; }
    public decimal WholesalePrice { get; set; }
    public string EAN13Barcode { get; set; } = string.Empty;
    public string ImagePath { get; set; }
}
