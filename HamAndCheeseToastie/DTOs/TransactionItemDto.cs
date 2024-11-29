using HamAndCheeseToastie.DTOs;

public class TransactionItemDto
{
    internal MauiProductDto Product;

    public int Id { get; set; }
    public int TransactionId { get; set; }       // Added to link with transaction
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    // Removed Product navigation property since we're decoupling
}
