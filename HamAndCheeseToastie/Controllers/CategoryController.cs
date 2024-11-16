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

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>A list of all categories</returns>
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

        /// <summary>
        /// Retrieves all categories for use in a MAUI app, with only CategoryID and CategoryName.
        /// </summary>
        /// <returns>A list of categories in a simplified format for the MAUI app</returns>
        [HttpGet("api/maui")]
        public async Task<IActionResult> GetAllCategoriesAsyncforMaui()
        {
            try
            {
                var categories = await _context.Categories
                    .Select(c => new MauiCategoryDto
                    {
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

        /// <summary>
        /// Retrieves products by category ID.
        /// </summary>
        /// <param name="category_id">The ID of the category to fetch products for</param>
        /// <returns>A list of products that belong to the specified category</returns>
        [HttpGet("{category_id}")]
        public async Task<IActionResult> GetProductsByCategory(int category_id)
        {
            var products = await _context.Products
                .Where(p => p.CategoryId == category_id)
                .ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="category">The category to create</param>
        /// <returns>The created category with a 201 status code</returns>
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
        
        
        /// <summary>
        /// Creates multiple new categories.
        /// </summary>
        /// <param name="categories">The list of categories to create</param>
        /// <returns>The created categories with a 201 status code</returns>
        [HttpPost("bulk_categories")]
        public async Task<IActionResult> CreateBulkCategoryAsync([FromBody] List<Category> categories)
        {
            if (categories == null || !categories.Any())
            {
                return BadRequest(new { message = "Invalid category data" });
            }

            try
            {
                foreach (var category in categories)
                {
                    if (string.IsNullOrEmpty(category.Name))
                    {
                        return BadRequest(new { message = "Invalid category data" });
                    }

                    _context.Categories.Add(category);
                }

                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, categories);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update</param>
        /// <param name="category">The updated category data</param>
        /// <returns>The updated category if successful, or a 404 if the category is not found</returns>
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

        /// <summary>
        /// Deletes a specific category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete</param>
        /// <returns>A message indicating whether the deletion was successful or not</returns>
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

        /// <summary>
        /// Searches for products by category name.
        /// </summary>
        /// <param name="categoryName">The name of the category to search for</param>
        /// <returns>A list of products that belong to the category with the specified name</returns>
        [HttpGet("search/{categoryName}")]
        public async Task<IActionResult> SearchProductsByCategoryName(string categoryName)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase));

            if (category == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            var products = await _context.Products
                .Where(p => p.CategoryId == category.Id)
                .ToListAsync();

            return Ok(products);
        }
    }
}
