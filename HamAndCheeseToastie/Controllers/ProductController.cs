using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;
using HamAndCheeseToastie.Services;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

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
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getSingleProduct(int id)
        {

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ID == id);

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

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
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
            productToUpdate.Category = product.Category;
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

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var products = _csvReader.ImportCsv(reader);

                if (products == null || !products.Any())
                {
                    return BadRequest("Invalid or empty CSV file");
                }

                // Bulk insert into the database
                _context.Products.AddRange(products);
                await _context.SaveChangesAsync();
            }

            return Ok(new { Message = "Products imported successfully" });
        }

    }
}
