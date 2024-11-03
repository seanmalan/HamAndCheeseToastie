using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("transaction_items")] // Set table name to lowercase for PostgreSQL compatibility
    public class TransactionItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("transaction_item_id")]
        public int TransactionItemId { get; set; } // Unique ID for the transaction item

        [Column("transaction_id")]
        public int TransactionId { get; set; } // Reference to the transaction

        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; } // Relationship to the main transaction

        [Column("product_id")]
        public int ProductId { get; set; } // Reference to the product

        [ForeignKey("ProductId")]
        public Product Product { get; set; } // Product details (relationship)

        [Column("quantity")]
        public int Quantity { get; set; } // Quantity of the item purchased

        [Column("unit_price")]
        public decimal UnitPrice { get; set; } // Price per unit

        [Column("total_price")]
        public decimal TotalPrice { get; set; } // Total price for this item (UnitPrice * Quantity)
    }
}
