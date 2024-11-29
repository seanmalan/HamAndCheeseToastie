public class StockAdjustmentDto
{
    internal string? ProductName;
    internal string? ProductWeight;
    internal int NewStockValue;
    internal int? UserId;

    // Product identification
    public int? Id { get; set; }
    public string Ean13Barcode { get; set; } = string.Empty;

    // Stock adjustment
    public int StockAdjustment { get; set; }
    public string? ReductionReason { get; set; }

    // Product details that might be updated
    public string? Name { get; set; }
    public string? BrandName { get; set; }
    public string? Weight { get; set; }
    public string? CategoryName { get; set; }
    public decimal? WholesalePrice { get; set; }
    public decimal? Price { get; set; }
}