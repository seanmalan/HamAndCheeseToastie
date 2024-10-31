using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Models;

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

        private bool TransactionExists(int id)
        {
            return _context.Transaction.Any(e => e.TransactionId == id);
        }
    }
}
