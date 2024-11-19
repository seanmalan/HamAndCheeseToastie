﻿using HamAndCheeseToastie.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Transaction
{
    [Key]
    [Column("transactionid")]
    public int TransactionId { get; set; }

    [Column("transactiondate")]
    public DateTime TransactionDate { get; set; }

    [Column("totalamount")]
    public decimal TotalAmount { get; set; }

    [Column("discount")]
    public decimal Discount { get; set; }

    [Column("paymentmethod")]
    public PaymentMethod PaymentMethod { get; set; }

    [Column("taxamount")]
    public decimal TaxAmount { get; set; }

    [Column("cashierid")] // This maps to the foreign key in PostgreSQL
    public int CashierId { get; set; } // Foreign key pointing to `Cashier.Id`

    [ForeignKey("CashierId")]
    public Cashier Cashier { get; set; } // Navigation property

    [Column("customerid")]
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }

    public ICollection<TransactionItem> TransactionItems { get; set; } = new List<TransactionItem>();
}
