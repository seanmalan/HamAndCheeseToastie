namespace HamAndCheeseToastie.DTOs
{
    public class MauiProductDto
    {

        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string ProductWeight { get; set; } = string.Empty;
        public string Category { get; set; }
        public int CurrentStockLevel { get; set; }
        public int MinimumStockLevel { get; set; }
        public decimal Price { get; set; }
        public decimal WholesalePrice { get; set; }
        public string EAN13Barcode { get; set; } = string.Empty;
        public string ImagePath { get; set; }
    }
}
