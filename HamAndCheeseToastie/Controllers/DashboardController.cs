using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public DashboardController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the total sales amount, considering discounts and tax.
        /// </summary>
        /// <returns>The total sales amount.</returns>
        [HttpGet("total_sales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTotalSales()
        {
            try
            {
                var totalAmount = await _context.Transaction.SumAsync(t => t.TotalAmount);
                var discount = await _context.Transaction.SumAsync(t => t.Discount);
                var tax = await _context.Transaction.SumAsync(t => t.TaxAmount);

                if (totalAmount == 0 && discount == 0 && tax == 0)
                    return NotFound("No sales data found.");

                var totalSales = totalAmount - discount + tax;
                return Ok(totalSales);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Gets the total number of transactions.
        /// </summary>
        /// <returns>The count of transactions.</returns>
        [HttpGet("total_transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> TotalTransactions()
        {
            var transactions = await _context.Transaction.CountAsync();
            if (transactions == 0)
                return NotFound("No transactions found.");

            return Ok(transactions);
        }

        /// <summary>
        /// Gets the 10 most recent transactions.
        /// </summary>
        /// <returns>A list of recent transactions.</returns>
        [HttpGet("recent_transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RecentTransactions()
        {
            var transactions = await _context.Transaction
                .OrderByDescending(t => t.TransactionDate)
                .Take(10)
                .ToListAsync();

            if (transactions == null || transactions.Count == 0)
                return NotFound("No recent transactions found.");

            return Ok(transactions);
        }

        /// <summary>
        /// Gets the total number of products.
        /// </summary>
        /// <returns>The count of products.</returns>
        [HttpGet("total_products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> TotalProducts()
        {
            var products = await _context.Products.CountAsync();
            if (products == 0)
                return NotFound("No products found.");

            return Ok(products);
        }

        /// <summary>
        /// Gets a list of products with low stock levels.
        /// </summary>
        /// <returns>A list of products with low stock levels.</returns>
        [HttpGet("available_products_levels")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AvailableStockLevels()
        {
            var lowStockProducts = await _context.Products
                .Where(p => p.CurrentStockLevel == 0 || p.CurrentStockLevel < p.MinimumStockLevel + 10)
                .Select(p => new
                {
                    ProductName = p.Name,
                    ProductId = p.ID,
                    CurrentStockLevel = p.CurrentStockLevel,
                    MinimumStockLevel = p.MinimumStockLevel
                })
                .ToListAsync();

            if (lowStockProducts == null || lowStockProducts.Count == 0)
                return NotFound("No low stock products found.");

            return Ok(lowStockProducts);
        }

        /// <summary>
        /// Gets the total number of active customers.
        /// </summary>
        /// <returns>The count of active customers.</returns>
        [HttpGet("total_customers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> TotalActiveCustomers()
        {
            var customers = await _context.Customer.CountAsync();
            if (customers == 0)
                return NotFound("No customers found.");

            return Ok(customers);
        }

        /// <summary>
        /// Gets insights on the top 5 customers based on total spending.
        /// </summary>
        /// <returns>A list of top 5 customers with their spending details.</returns>
        [HttpGet("customer_insights")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CustomerInsights()
        {
            var topCustomers = await _context.Transaction
                .GroupBy(t => t.CustomerId)
                .Select(group => new
                {
                    CustomerId = group.Key,
                    TotalSpent = group.Sum(t => t.TotalAmount),
                    LastTransactionDate = group.Max(t => t.TransactionDate)
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(5)
                .Join(_context.Customer,
                      g => g.CustomerId,
                      c => c.CustomerId,
                      (g, c) => new
                      {
                          CustomerId = c.CustomerId,
                          FirstName = c.FirstName,
                          LastName = c.LastName,
                          TotalSpent = g.TotalSpent,
                          LastTransactionDate = g.LastTransactionDate
                      })
                .ToListAsync();

            if (topCustomers == null || topCustomers.Count == 0)
                return NotFound("No customer insights found.");

            return Ok(topCustomers);
        }
    }
}
