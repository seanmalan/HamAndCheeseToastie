using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;
using HamAndCheeseToastie.DTOs;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauiController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public MauiController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllProductsMaui()
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

        [HttpGet("product/search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string query,
            [FromQuery] string category = null)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Query parameter is required.");
            }

            // Build the queryable collection of products
            var productsQuery = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            // Apply search query filter
            productsQuery = productsQuery.Where(p =>
                p.Name.Contains(query) ||
                p.BrandName.Contains(query) ||
                p.EAN13Barcode.Contains(query));

            // Apply category filter if specified
            if (!string.IsNullOrEmpty(category))
            {
                productsQuery = productsQuery.Where(p => p.Category.Name == category);
            }

            var products = await productsQuery
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

            if (!products.Any())
            {
                return NotFound("No products found matching the criteria.");
            }

            return Ok(products);
        }




        // GET: api/Maui/Customer
        /// <summary>
        /// Retrieves a simplified list of customers for Maui clients.
        /// </summary>
        /// <response code="200">Returns the list of customers formatted for Maui clients.</response>
        [HttpGet("Customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerMaui()
        {
            var customers = await _context.Customer
                .Select(c => new MauiCustomerDto
                {
                    Id = c.CustomerId,
                    CustomerId = c.CustomerId,
                    Barcode = c.FirstName,
                    CustomerName = c.FirstName,
                    Surname = c.LastName,
                    Email = c.Email,
                    Phone = c.PhoneNumber,
                    IsMember = c.IsLoyaltyMember
                })
                .ToListAsync();

            return Ok(customers);
        }





        /// <summary>
        /// Retrieves filtered transactions based on date range and count.
        /// </summary>
        /// <param name="dateFrom">Start date of the transaction period.</param>
        /// <param name="dateTo">End date of the transaction period.</param>
        /// <param name="count">Number of transactions to retrieve.</param>
        /// <response code="200">Returns the filtered list of transactions.</response>
        [HttpGet("Transactions")]
        public async Task<IActionResult> GetTransactions(
    [FromQuery] string dateFrom = null,
    [FromQuery] string dateTo = null,
    [FromQuery] int? count = null)
        {
            if (!DateTime.TryParse(dateFrom, out var fromDate))
            {
                fromDate = DateTime.Now.AddDays(-30);
            }

            if (!DateTime.TryParse(dateTo, out var toDate))
            {
                toDate = DateTime.Now;
            }

            if (fromDate > toDate)
            {
                return BadRequest("Invalid date range: 'dateFrom' must be earlier than 'dateTo'.");
            }

            var transactionCount = count ?? 10;

            var transactions = await _context.Transaction
                .Select(t => new
                {
                    t.TransactionId,
                    t.TransactionDate,
                    t.TotalAmount,
                    t.Discount,
                    t.PaymentMethod,
                    t.TaxAmount,
                    t.CashierId,
                    t.CustomerId
                })
                .Where(t => t.TransactionDate >= fromDate && t.TransactionDate <= toDate)
                .OrderByDescending(t => t.TransactionDate)
                .Take(transactionCount)
                .ToListAsync();

            if (!transactions.Any())
            {
                return NotFound("No transactions found for the given criteria.");
            }

            return Ok(transactions);
        }





        [HttpGet("Transactions/{transactionId}/Items")]
        public async Task<IActionResult> GetTransactionItems(int transactionId)
        {
            var items = await _context.TransactionItem
                .Where(i => i.TransactionId == transactionId)
                .ToListAsync();

            if (!items.Any())
            {
                return NotFound("No items found for this transaction.");
            }

            return Ok(items);
        }




        /// <summary>
        /// Retrieves filtered transactions based on date range and count.
        /// </summary>
        /// <param name="dateFrom">Start date of the transaction period.</param>
        /// <param name="dateTo">End date of the transaction period.</param>
        /// <param name="count">Number of transactions to retrieve.</param>
        /// <response code="200">Returns the filtered list of transactions.</response>
        /// <summary>
        /// Retrieves a transaction by ID along with its associated transaction items.
        /// </summary>
        /// <param name="id">The ID of the transaction.</param>
        /// <response code="200">Returns the transaction with its items.</response>
        /// <response code="404">If the transaction is not found.</response>
        [HttpGet("Transactions/{id}/Items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionWithItems(int id)
        {
            // Query the database for the transaction by ID, including its items
            var transaction = await _context.Transaction
                .Include(t => t.TransactionItems)
                    .ThenInclude(ti => ti.Product) // Include product details if needed
                .Include(t => t.Customer) // Include customer details if needed
                .FirstOrDefaultAsync(t => t.TransactionId == id);

            // If the transaction is not found, return a 404 Not Found response
            if (transaction == null)
            {
                return NotFound($"Transaction with ID {id} was not found.");
            }

            // Return the transaction with its items
            return Ok(transaction);
        }


    }
}
