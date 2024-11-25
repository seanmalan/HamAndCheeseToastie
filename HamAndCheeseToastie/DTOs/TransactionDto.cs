using HamAndCheeseToastie.Models;

public class TransactionDto
{
    public int TransactionId { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TaxAmount { get; set; }
    public int UserId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }

    // List of transaction items
    public List<TransactionItemDto> TransactionItems { get; set; } = new();
}
