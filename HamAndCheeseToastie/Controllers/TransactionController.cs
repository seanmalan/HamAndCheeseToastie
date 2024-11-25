using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.DTOs;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TransactionController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactions(
        [FromQuery] DateTime? dateFrom = null,
        [FromQuery] DateTime? dateTo = null)
        {
            // Default values if dates are not provided
            var fromDate = (dateFrom ?? DateTime.Now.AddDays(-30)).ToUniversalTime();
            var toDate = (dateTo ?? DateTime.Now).ToUniversalTime();

            // Validate the date range
            if (fromDate > toDate)
            {
                return BadRequest("Invalid date range: 'dateFrom' must be earlier than 'dateTo'.");
            }

            // Fetch transactions within the date range
            var transactions = await _context.Transaction
                .Where(t => t.TransactionDate >= fromDate && t.TransactionDate <= toDate)
                .OrderByDescending(t => t.TransactionDate) // Sort by date, most recent first
                .Include(t => t.Customer)
                .Include(t => t.TransactionItems)
                .Select(t => new
                {
                    t.TransactionId,
                    t.TransactionDate,
                    t.TotalAmount,
                    t.Discount,
                    t.PaymentMethod,
                    t.TaxAmount,
                    t.Customer,
                    t.UserId,
                    TransactionItems = t.TransactionItems.Select(item => new
                    {
                        item.Id,
                        item.Product,
                        item.Quantity,
                        item.UnitPrice
                    })
                })
                .ToListAsync();

            if (transactions == null || !transactions.Any())
            {
                return NotFound("No transactions found for the specified date range.");
            }

            return Ok(transactions);
        }


        // GET: api/Transaction/5
        // GET: api/Transaction/5
        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDto>> GetTransaction(int id)
        {
            var transaction = await _context.Transaction
                .Include(t => t.TransactionItems) // Include TransactionItems
                    .ThenInclude(ti => ti.Product) // Include Product details
                .Where(t => t.TransactionId == id)
                .Select(t => new TransactionDto
                {
                    TransactionId = t.TransactionId,
                    TransactionDate = t.TransactionDate,
                    TotalAmount = t.TotalAmount,
                    Discount = t.Discount,
                    TaxAmount = t.TaxAmount,
                    UserId = t.UserId,
                    PaymentMethod = t.PaymentMethod,
                    TransactionItems = t.TransactionItems.Select(ti => new TransactionItemDto
                    {
                        Id = ti.Id,
                        ProductId = ti.ProductId, // Ensure ProductId matches the database column
                        Quantity = ti.Quantity,
                        UnitPrice = ti.UnitPrice,
                        TotalPrice = ti.TotalPrice,
                        Product = ti.Product != null ? new ProductDto
                        {
                            ProductId = ti.Product.ID,
                            Name = ti.Product.Name,
                            BrandName = ti.Product.BrandName,
                            Weight = ti.Product.Weight
                        } : null
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound(new { Message = $"Transaction with ID {id} not found." });
            }

            return Ok(transaction);
        }




        // PUT: api/Transaction/5
        // PUT: api/Transaction/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return BadRequest(new { Message = "Transaction ID mismatch." });
            }

            // Attach customer if it's a navigation property
            if (transaction.Customer != null)
            {
                _context.Entry(transaction.Customer).State = EntityState.Modified;
            }

            // Attach transaction items if any
            if (transaction.TransactionItems != null)
            {
                foreach (var item in transaction.TransactionItems)
                {
                    _context.Entry(item).State = item.Id == 0
                        ? EntityState.Added
                        : EntityState.Modified;
                }
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Transaction.Any(e => e.TransactionId == id))
                {
                    return NotFound(new { Message = "Transaction not found for update." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Transaction
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction([FromBody] Transaction transaction)
        {
            // Validate the transaction (you can add custom validation here)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Transaction.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.TransactionId }, transaction);
        }

        // DELETE: api/Transaction/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);
            if (transaction == null)
            {
                return NotFound(new { Message = $"Transaction with ID {id} not found." });
            }

            _context.Transaction.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //retrive transactions from a specified date and limit
        // GET: api/Transaction
        [HttpGet("api/maui/transactions")]
        public async Task<IActionResult> GetTransactions(DateTime? dateFrom = null, DateTime? dateTo = null, int count = 100)
        {
            // Set default date range if not provided
            dateFrom ??= DateTime.MinValue;
            dateTo ??= DateTime.MaxValue;

            // Fetch transactions within the specified date range and limit the count
            var transactions = await _context.Transaction
                .Where(t => t.TransactionDate >= dateFrom && t.TransactionDate <= dateTo)
                .OrderByDescending(t => t.TransactionDate)
                .Take(count)
                .Select(t => new MauiTransactionDto()
                {
                    Id = t.TransactionId,
                    DateTime = t.TransactionDate,
                    TotalAmount = t.TotalAmount,
                    Discount = t.Discount,
                    //PaymentMethod = t.PaymentMethod,
                    GServiceTax = t.TaxAmount,
                    CustomerId = t.CustomerId
                })
                .ToListAsync();

            return Ok(transactions);
        }


        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }
    }
}
