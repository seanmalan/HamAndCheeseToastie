using HamAndCheeseToastie.Models;

public class TransactionDto
{
    public int TransactionId { get; set; }  // Added back for GET/PUT operations
    public DateTime TransactionDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public decimal TaxAmount { get; set; }
    public int UserId { get; set; }
    public int CustomerId { get; set; }
    public string PaymentMethod { get; set; }
    public List<TransactionItemDto> TransactionItems { get; set; } = new List<TransactionItemDto>();  // Needed for GET operations
}
