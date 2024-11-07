using CsvHelper.Configuration;
using HamAndCheeseToastie.DTOs;

public class ProductMap : ClassMap<ProductDto>
{
    public ProductMap()
    {
        Map(p => p.Name).Name("ProductName");
        Map(p => p.BrandName).Name("BrandName");
        Map(p => p.Weight).Name("ProductWeight");
        Map(p => p.Category_id).Name("Category");
        Map(p => p.CurrentStockLevel).Name("CurrentStockLevel");
        Map(p => p.MinimumStockLevel).Name("MinimumStockLevel");
        Map(p => p.Price).Name("Price");
        Map(p => p.WholesalePrice).Name("WholesalePrice");
        Map(p => p.EAN13Barcode).Name("EAN13Barcode");
        Map(p => p.ImagePath).Name("ImagePath");
    }
}
