using HamAndCheeseToastie.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("transactions")] // Adding table name for consistency
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

    [Column("userid")]
    public int UserId { get; set; }

    [Column("customerid")]
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; }
}