using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;
using HamAndCheeseToastie.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products
            .Include(p => p.Category) // Ensure Category is correctly included as a navigation property
            .Select(p => new ProductDto
            {
                ID = p.ID,
                Name = p.Name,
                BrandName = p.BrandName,
                Weight = p.Weight,
                Category_id = p.Category_id,  // The foreign key ID
                CategoryName = p.Category.Name, // Get the category name from the navigation property
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
        public async Task<IActionResult> getSingleProduct(int id)
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
                    Category_id = p.Category_id,
                    CategoryName = p.Category.Name, // Get Category Name
                    CurrentStockLevel = p.CurrentStockLevel,
                    MinimumStockLevel = p.MinimumStockLevel,
                    Price = p.Price,
                    WholesalePrice = p.WholesalePrice,
                    EAN13Barcode = p.EAN13Barcode,
                    ImagePath = p.ImagePath
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(); // Return 404 if product is not found
            }

            return Ok(product);
        }


        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Product product, IFormFile imageFile)
        {
            if (product == null)
            {
                return BadRequest("Product data is required");
            }

            // Handle image upload
            if (imageFile != null && imageFile.Length > 0)
            {
                var filePath = Path.Combine(_environment.WebRootPath, "images", imageFile.FileName);

                // Save image to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Store the image path in the database
                product.ImagePath = $"/images/{imageFile.FileName}";
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(getSingleProduct), new { id = product.ID }, product);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product, IFormFile imageFile)
        {
            if (product == null)
            {
                return BadRequest("Product is null"); // Return 400 if product is null
            }

            var productToUpdate = await _context.Products.FirstOrDefaultAsync(p => p.ID == id);

            if (productToUpdate == null)
            {
                return NotFound(); // Return 404 if product is not found
            }

            // Update product fields
            productToUpdate.Name = product.Name;
            productToUpdate.BrandName = product.BrandName;
            productToUpdate.Weight = product.Weight;
            productToUpdate.Category_id = product.Category_id;
            productToUpdate.CurrentStockLevel = product.CurrentStockLevel;
            productToUpdate.MinimumStockLevel = product.MinimumStockLevel;
            productToUpdate.Price = product.Price;
            productToUpdate.WholesalePrice = product.WholesalePrice;
            productToUpdate.EAN13Barcode = product.EAN13Barcode;

            if (imageFile != null && imageFile.Length > 0)
            {
                var filePath = Path.Combine(_environment.WebRootPath, "images", imageFile.FileName);

                // Save the new image
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Update image path
                productToUpdate.ImagePath = $"/images/{imageFile.FileName}";
            }

            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 No Content as the update is successful
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("csv-upload")]
        public async Task<IActionResult> UploadCSV(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            var productsToInsert = new List<Product>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // Assuming your CSV file headers match your Product properties
                csv.Context.RegisterClassMap<ProductMap>(); // Map Product columns
                var records = csv.GetRecords<ProductDto>().ToList();

                // Fetch all existing category IDs to validate against
                var existingCategoryIds = await _context.Categories
                    .Select(c => c.Id)
                    .ToListAsync();

                foreach (var record in records)
                {
                    if (!existingCategoryIds.Contains(record.Category_id))
                    {
                        Console.WriteLine($"Invalid Category_id {record.Category_id} for product {record.Name}");
                        continue;
                    }

                    // Check if a product with the same unique identifiers already exists
                    var existingProduct = await _context.Products
                        .FirstOrDefaultAsync(p => p.Name == record.Name && p.Category_id == record.Category_id);

                    if (existingProduct == null)
                    {
                        var product = new Product
                        {
                            Name = record.Name,
                            BrandName = record.BrandName,
                            Weight = record.Weight,
                            Category_id = record.Category_id,
                            CurrentStockLevel = record.CurrentStockLevel,
                            MinimumStockLevel = record.MinimumStockLevel,
                            Price = record.Price,
                            WholesalePrice = record.WholesalePrice,
                            EAN13Barcode = record.EAN13Barcode,
                            ImagePath = record.ImagePath
                        };
                        productsToInsert.Add(product);
                    }
                }
            }

            // Bulk insert into the database
            _context.Products.AddRange(productsToInsert);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Products imported successfully" });
        }
    }
}
