using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("transactions")] // Set table name to lowercase for PostgreSQL compatibility
    public class Transaction
    {
        [Key]
        [Column("transaction_id")] // Set column name to lowercase
        public int TransactionId { get; set; } // Unique ID for the transaction

        [Column("transaction_date")]
        public DateTime TransactionDate { get; set; } // Date and time of the transaction

        [Column("total_amount")]
        public decimal TotalAmount { get; set; } // Total amount of the transaction

        [Column("discount")]
        public decimal Discount { get; set; } // Discount applied to the transaction

        [Column("payment_method")]
        public PaymentMethod PaymentMethod { get; set; } // Enum or reference to payment method (Cash, Credit, etc.)

        [Column("tax_amount")]
        public decimal TaxAmount { get; set; } // Total tax applied

        [Column("cashier_id")]
        public int CashierId { get; set; } // Reference to the employee handling the transaction

        [ForeignKey("CashierId")]
        public Cashier Cashier { get; set; } // Cashier details (relationship)

        [Column("customer_id")]
        public int CustomerId { get; set; } // Reference to customer (if available)

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } // Customer details (relationship)

        // Related collections
        public ICollection<TransactionItem> TransactionItems { get; set; } = new List<TransactionItem>(); // Items purchased in this transaction
    }
}
