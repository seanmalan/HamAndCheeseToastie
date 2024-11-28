using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("transaction_items")]
public class TransactionItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("transactionid")]
    public int TransactionId { get; set; }

    [Column("productid")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("unitprice")]
    public decimal UnitPrice { get; set; }

    [Column("totalprice")]
    public decimal TotalPrice { get; set; }

    // Remove the navigation properties to Transaction and Product
}