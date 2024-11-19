using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;
using HamAndCheeseToastie.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using HamAndCheeseToastie.DTOs;
using System.Globalization;
using CsvHelper;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ICsvReader _csvReader;
        private readonly IWebHostEnvironment _environment;

        public ProductController(DatabaseContext context, ICsvReader csvReader, IWebHostEnvironment environment)
        {
            _context = context;
            _csvReader = csvReader;
            _environment = environment;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    ID = p.ID,
                    Name = p.Name,
                    BrandName = p.BrandName,
                    Weight = p.Weight,
                    CategoryId = p.CategoryId, // Ensure this matches the property correctly
                    CategoryName = p.Category.Name, // CategoryName mapped to the Category's Name property
                    CurrentStockLevel = p.CurrentStockLevel,
                    MinimumStockLevel = p.MinimumStockLevel,
                    Price = p.Price,
                    WholesalePrice = p.WholesalePrice,
                    EAN13Barcode = p.EAN13Barcode,
                    ImagePath = p.ImagePath
                })
                .ToListAsync();


            return Ok(products);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSingleProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.ID == id)
                .Select(p => new ProductDto
                {
                    ID = p.ID,
                    Name = p.Name,
                    BrandName = p.BrandName,
                    Weight = p.Weight,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    CurrentStockLevel = p.CurrentStockLevel,
                    MinimumStockLevel = p.MinimumStockLevel,
                    Price = p.Price,
                    WholesalePrice = p.WholesalePrice,
                    EAN13Barcode = p.EAN13Barcode,
                    ImagePath = p.ImagePath
                })
                .FirstOrDefaultAsync();

            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct([FromForm] Product product, IFormFile imageFile)
        {
            if (product == null) return BadRequest("Product data is required");

            product.ImagePath = await SaveImageFileAsync(imageFile);
            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                var filePath = Path.Combine(_environment.WebRootPath, "images", imageFile.FileName);

                // Save image to the specified path
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Store the image path in the database
                product.ImagePath = $"/images/{imageFile.FileName}";
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingleProduct), new { id = product.ID }, product);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product, IFormFile imageFile)
        {
            if (product == null) return BadRequest("Product data is required");

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ID == id);
            if (existingProduct == null) return NotFound();

            existingProduct.Name = product.Name;
            existingProduct.BrandName = product.BrandName;
            existingProduct.Weight = product.Weight;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.CurrentStockLevel = product.CurrentStockLevel;
            existingProduct.MinimumStockLevel = product.MinimumStockLevel;
            existingProduct.Price = product.Price;
            existingProduct.WholesalePrice = product.WholesalePrice;
            existingProduct.EAN13Barcode = product.EAN13Barcode;
            existingProduct.ImagePath = await SaveImageFileAsync(imageFile, existingProduct.ImagePath);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("csv-upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadCSV(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("No file uploaded");

            var productsToInsert = await ParseCsvFileAsync(file);
            _context.Products.AddRange(productsToInsert);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Products imported successfully" });
        }

        private async Task<string> SaveImageFileAsync(IFormFile imageFile, string existingFilePath = null)
        {
            if (imageFile == null || imageFile.Length == 0) return existingFilePath;

            var filePath = Path.Combine(_environment.WebRootPath, "images", imageFile.FileName);
            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"/images/{imageFile.FileName}";
        }

        private async Task<List<Product>> ParseCsvFileAsync(IFormFile file)
        {
            var productsToInsert = new List<Product>();

            using var reader = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<ProductMap>();

            var records = csv.GetRecords<ProductDto>().ToList();
            var existingCategoryIds = await _context.Categories.Select(c => c.Id).ToListAsync();

            foreach (var record in records)
            {
                if (!existingCategoryIds.Contains(record.CategoryId)) continue;

                if (await _context.Products.AnyAsync(p => p.Name == record.Name && p.CategoryId == record.CategoryId)) continue;

                var product = new Product
                {
                    Name = record.Name,
                    BrandName = record.BrandName,
                    Weight = record.Weight,
                    CategoryId = record.CategoryId,
                    CurrentStockLevel = record.CurrentStockLevel,
                    MinimumStockLevel = record.MinimumStockLevel,
                    Price = record.Price,
                    WholesalePrice = record.WholesalePrice,
                    EAN13Barcode = record.EAN13Barcode,
                    ImagePath = record.ImagePath
                };
                productsToInsert.Add(product);
            }

            return productsToInsert;
        }
    }
}
