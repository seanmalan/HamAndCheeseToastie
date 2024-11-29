namespace HamAndCheeseToastie.DTOs
{
    public class TransactionCreateDto  // New DTO specifically for creating transactions
    {
        public DateTime TransactionDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal TaxAmount { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
