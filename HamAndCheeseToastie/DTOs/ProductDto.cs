namespace HamAndCheeseToastie.DTOs
{
    public class ProductDto
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BrandName { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty; // Add this field
        public int CurrentStockLevel { get; set; }
        public int MinimumStockLevel { get; set; }
        public decimal Price { get; set; }
        public decimal WholesalePrice { get; set; }
        public string EAN13Barcode { get; set; } = string.Empty;
        public string ImagePath { get; set; }
    }

}
