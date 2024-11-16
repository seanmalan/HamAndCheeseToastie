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
        [Column("transactionid")] // Map to 'transaction_id' column in PostgreSQL
        public int TransactionId { get; set; } // Unique ID for the transaction

        [Column("transactiondate")] // Map to 'transaction_date' column in PostgreSQL
        public DateTime TransactionDate { get; set; } // Date and time of the transaction

        [Column("totalamount")] // Map to 'total_amount' column in PostgreSQL
        public decimal TotalAmount { get; set; } // Total amount of the transaction

        [Column("discount")] // Map to 'discount' column in PostgreSQL
        public decimal Discount { get; set; } // Discount applied to the transaction

        [Column("paymentmethod")] // Map to 'payment_method' column in PostgreSQL
        public PaymentMethod PaymentMethod { get; set; } // Enum or reference to payment method (Cash, Credit, etc.)

        [Column("taxamount")] // Map to 'tax_amount' column in PostgreSQL
        public decimal TaxAmount { get; set; } // Total tax applied

        [Column("cashierid")] // Map to 'cashier_id' column in PostgreSQL
        public int CashierId { get; set; } // Reference to the employee handling the transaction

        [ForeignKey("CashierId")]
        public Cashier Cashier { get; set; } // Cashier details (relationship)

        [Column("customerid")] // Map to 'customer_id' column in PostgreSQL
        public int CustomerId { get; set; } // Reference to customer (if available)

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; } // Customer details (relationship)

        // Related collections
        public ICollection<TransactionItem> TransactionItems { get; set; } = new List<TransactionItem>(); // Items purchased in this transaction
    }
}
