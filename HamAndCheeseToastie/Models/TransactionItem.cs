namespace HamAndCheeseToastie.Models
{
    public class TransactionItem
    {
        public int TransactionItemId { get; set; }              // Unique ID for the transaction item
        public int TransactionId { get; set; }                  // Reference to the transaction
        public Transaction Transaction { get; set; }            // Relationship to the main transaction
        public int ProductId { get; set; }                      // Reference to the product
        public Product Product { get; set; }                    // Product details (relationship)
        public int Quantity { get; set; }                       // Quantity of the item purchased
        public decimal UnitPrice { get; set; }                  // Price per unit
        public decimal TotalPrice { get; set; }                 // Total price for this item (UnitPrice * Quantity)
    }

}
