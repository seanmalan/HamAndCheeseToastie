namespace HamAndCheeseToastie.DTOs
{
    public class MauiCustomerDto
    {

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string? Barcode { get; set; }
        public string? CustomerName { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public bool IsMember { get; set; }

        public string? BarcodeImage { get; set; }
    }
}
