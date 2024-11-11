using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.DTOs;
using HamAndCheeseToastie.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CategoryController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _context.Categories.ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        //GET from the maui app
        // GET: api/Category
        [HttpGet("api/maui")]
        public async Task<IActionResult> GetAllCategoriesAsyncforMaui()
        {
            try
            {
                var categories = await _context.Categories
                    .Select(c => new MauiCategoryDto{
                       CategoryID = c.Id, 
                        CategoryName = c.Name
                    })
                    .ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // GET: api/Product/category/{category_id}
        [HttpGet("{category_id}")]
        public async Task<IActionResult> GetProductsByCategory(int category_id)
        {
            var products = await _context.Products
                .Where(p => p.CategoryId == category_id)
                .ToListAsync();

            return Ok(products);
        }


        // POST: api/Category
        [HttpPost]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] Category category)
        {
            if (category == null || string.IsNullOrEmpty(category.Name))
            {
                return BadRequest(new { message = "Invalid category data" });
            }

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, category);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // PUT: api/Category/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoryAsync(int id, [FromBody] Category category)
        {
            if (id != category.Id || category == null || string.IsNullOrEmpty(category.Name))
            {
                return BadRequest(new { message = "Invalid category data" });
            }

            try
            {
                var existingCategory = await _context.Categories.FindAsync(id);
                if (existingCategory == null)
                {
                    return NotFound(new { message = "Category not found" });
                }

                existingCategory.Name = category.Name;
                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();
                return Ok(existingCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        // DELETE: api/Category/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound(new { message = "Category not found" });
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        // do a  search for the products using the category name as a string not the id.
    }
}
