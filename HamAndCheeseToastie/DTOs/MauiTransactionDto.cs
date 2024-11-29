namespace HamAndCheeseToastie.DTOs
{
    public class MauiTransactionDto
    {
        internal int UserId;
        internal int TransactionId;

        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal Discount { get; set; }

        public string? PaymentMethod { get; set; }

        public decimal GServiceTax { get; set; }

        public int? CustomerId { get; set; }

        public string? Receipt { get; set; }

    }
}
