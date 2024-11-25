using HamAndCheeseToastie.DTOs;

public class TransactionItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }

    // Nested DTO for Product
    public ProductDto? Product { get; set; }
}
