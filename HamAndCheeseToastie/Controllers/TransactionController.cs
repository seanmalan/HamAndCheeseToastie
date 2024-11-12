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

        // GET: api/Transaction
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _context.Transaction.ToListAsync();
            return Ok(transactions);
        }

        // GET: api/Transaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transaction.FindAsync(id);

            if (transaction == null)
            {
                return NotFound(new { Message = $"Transaction with ID {id} not found." });
            }

            return Ok(transaction);
        }

        // PUT: api/Transaction/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, [FromBody] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return BadRequest(new { Message = "Transaction ID mismatch." });
            }

            // Validate the transaction (you can add custom validation here)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound(new { Message = $"Transaction with ID {id} not found." });
                }
                throw; // Rethrow if there's a concurrency issue not related to existence
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
