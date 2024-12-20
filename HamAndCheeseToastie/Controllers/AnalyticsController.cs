﻿using HamAndCheeseToastie.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : Controller
    {
        private readonly DatabaseContext _context;

        public AnalyticsController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves analytics data based on the specified dataset and optional period filter.
        /// </summary>
        /// <param name="dataset">The name of the dataset to fetch analytics for (e.g., "transaction", "category", "product")</param>
        /// <param name="period">The optional time period filter (e.g., "week", "fortnight", "month", "year")</param>
        /// <returns>Returns the analytics data based on the selected dataset and period filter</returns>
        [HttpGet("data")]
        public async Task<IActionResult> GetAnalyticsData(string dataset, string period = null)
        {
            if (string.IsNullOrEmpty(dataset))
            {
                return BadRequest("Dataset is required.");
            }

            DateTime? startDate = null;
            if (!string.IsNullOrEmpty(period))
            {
                startDate = period.ToLower() switch
                {
                    "week" => DateTime.UtcNow.AddDays(-7),
                    "fortnight" => DateTime.UtcNow.AddDays(-14),
                    "month" => DateTime.UtcNow.AddMonths(-1),
                    "year" => DateTime.UtcNow.AddYears(-1),
                    _ => null
                };

                if (startDate == null)
                {
                    return BadRequest("Invalid period.");
                }
            }

            // Fetch data based on the selected dataset
            object data = dataset.ToLower() switch
            {
                "transaction" => await _context.Transaction
                    .Where(t => t.TransactionDate >= startDate)
                    .GroupBy(t => t.TransactionDate.Date)
                    .Select(g => new
                    {
                        Date = g.Key,
                        TotalInflow = g.Sum(t => t.TotalAmount),
                        TotalOutflow = g.Sum(t => t.Discount + t.TaxAmount)
                    }).ToListAsync(),

                "category" => await _context.Categories
                    .Select(c => new
                    {
                        CategoryName = c.Name,
                        ProductCount = c.Name.Count() // Assuming a one-to-many relationship
                    }).ToListAsync(),

                "product" => await _context.Products
                .Where(p => p.CurrentStockLevel > 0)
                    .Select(p => new
                    {
                        ProductName = p.Name,
                        CurrentStockLevel = p.CurrentStockLevel,
                        MinimumStockLevel = p.MinimumStockLevel
                    }).ToListAsync(),

                _ => null
            };

            if (data == null)
            {
                return NotFound($"No data found for dataset: {dataset}");
            }

            return Ok(data);
        }
    }
}
