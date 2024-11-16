using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("transaction_items")] // Set table name to lowercase for PostgreSQL compatibility
    public class TransactionItem
    {
        [Key]
        [Column("id")] // Map to 'id' column in PostgreSQL
        public int Id { get; set; }

        [Column("transactionid")] // Map to 'transaction_id' column in PostgreSQL
        public int TransactionId { get; set; } // Reference to the transaction

        [ForeignKey("TransactionId")]
        public Transaction Transaction { get; set; } // Relationship to the main transaction

        [Column("productid")] // Map to 'product_id' column in PostgreSQL
        public int ProductId { get; set; } // Reference to the product

        [ForeignKey("ProductId")]
        public Product Product { get; set; } // Product details (relationship)

        [Column("quantity")] // Map to 'quantity' column in PostgreSQL
        public int Quantity { get; set; } // Quantity of the item purchased

        [Column("unitprice")] // Map to 'unit_price' column in PostgreSQL
        public decimal UnitPrice { get; set; } // Price per unit

        [Column("totalprice")] // Map to 'total_price' column in PostgreSQL
        public decimal TotalPrice { get; set; } // Total price for this item (UnitPrice * Quantity)
    }
}