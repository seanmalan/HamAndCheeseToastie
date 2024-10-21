using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProductController(DatabaseContext context)
        {
            _context = context;
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
        public async Task<IActionResult> Post([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null"); // Return 400 if product is null
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, product); // Return 201 Created with the product

        }



        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Product product)
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
            productToUpdate.Description = product.Description;
            productToUpdate.Weight = product.Weight;
            productToUpdate.Category = product.Category;
            productToUpdate.CurrentStockLevel = product.CurrentStockLevel;
            productToUpdate.MinimumStockLevel = product.MinimumStockLevel;
            productToUpdate.Price = product.Price;
            productToUpdate.WholesalePrice = product.WholesalePrice;
            productToUpdate.EAN13Barcode = product.EAN13Barcode;

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
    }
}
