using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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

        [HttpGet("total_sales")]
        public async Task<IActionResult> GetTotalSales()
        {
            // Use async operations with await
            var total_amount = await _context.Transactions.SumAsync(t => t.TotalAmount);
            var discount = await _context.Transactions.SumAsync(t => t.Discount);
            var tax = await _context.Transactions.SumAsync(t => t.TaxAmount);

            var total_sales = total_amount - discount + tax;

            return Ok(total_sales);
        }

        [HttpGet("total_transactions")] // Change route to avoid conflicts
        public async Task<IActionResult> TotalTransactions()
        {
            var transactions = await _context.Transactions.CountAsync(); // Use async version
            return Ok(transactions);
        }

        [HttpGet("recent_transactions")] // Change route to avoid conflicts
        public async Task<IActionResult> RecentTransactions()
        {
            var transactions = await _context.Transactions
                .OrderByDescending(t => t.TransactionDate) // Order by most recent first
                .Take(10)                                  // Take the last 10 transactions
                .ToListAsync();                            // Execute query and fetch results asynchronously

            return Ok(transactions);                       // Return the list of transactions
        }


        [HttpGet("total_products")] // Change route to avoid conflicts
        public async Task<IActionResult> TotalProducts()
        {
            var products = await _context.Products.CountAsync(); // Use async version
            return Ok(products);
        }



        [HttpGet("total_customers")] // Change route to avoid conflicts
        public async Task<IActionResult> TotalActiveCustomers()
        {
            var customers = await _context.Customers.CountAsync(); // Use async version
            return Ok(customers);
        }



        [HttpGet("customer_insights")] // Change route to avoid conflicts
        public async Task<IActionResult> CustomerInsights()
        {
            var topCustomers = await _context.Transactions
                .GroupBy(t => t.CustomerId)
                .Select(group => new
                {
                    CustomerId = group.Key,
                    TotalSpent = group.Sum(t => t.TotalAmount),
                    LastTransactionDate = group.Max(t => t.TransactionDate) // Get the most recent transaction date
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(5)
                .Join(_context.Customers,
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

            return Ok(topCustomers);
        }


    }
}
