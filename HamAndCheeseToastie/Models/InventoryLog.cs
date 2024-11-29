using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HamAndCheeseToastie.Models
{
    [Table("inventory_logs")]
    public class InventoryLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int LogID { get; set; }

        [Column("product_id")]
        public int ProductID { get; set; }

        [Column("barcode")]
        public string Barcode { get; set; } = string.Empty;

        [Column("change_type")]
        public string? ChangeType { get; set; }

        [Column("stock_old_value")]
        public string? StockOldValue { get; set; }

        [Column("stock_new_value")]
        public string? StockNewValue { get; set; }

        [Column("reduction_reason")]
        public string? ReductionReason { get; set; }

        [Column("name_old_value")]
        public string? NameOldValue { get; set; }

        [Column("name_new_value")]
        public string? NameNewValue { get; set; }

        [Column("brand_old_value")]
        public string? BrandOldValue { get; set; }

        [Column("brand_new_value")]
        public string? BrandNewValue { get; set; }

        [Column("category_old_value")]
        public string? CategoryOldValue { get; set; }

        [Column("category_new_value")]
        public string? CategoryNewValue { get; set; }

        [Column("price_old_value")]
        public decimal PriceOldValue { get; set; }

        [Column("price_new_value")]
        public decimal PriceNewValue { get; set; }

        [Column("wholesale_price_old_value")]
        public decimal WholesalePriceOldValue { get; set; }

        [Column("wholesale_price_new_value")]
        public decimal WholesalePriceNewValue { get; set; }

        [Column("user_id")]
        public int? UserId { get; set; }

        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        // Navigation property
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}
