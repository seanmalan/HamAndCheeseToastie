using HamAndCheeseToastie.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("products")]
public class Product
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int ID { get; set; }

    [Column("name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [Column("brand_name")]
    public string BrandName { get; set; } = string.Empty;

    [Column("weight")]
    public string Weight { get; set; } = string.Empty;

    [Column("category_id")]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; } = null!;

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

    [NotMapped]
    public string CategoryName => Category?.Name ?? string.Empty;
}