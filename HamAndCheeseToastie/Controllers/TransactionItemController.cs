using System;
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
    public class TransactionItemController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public TransactionItemController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/TransactionItem
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionItem>>> GetTransactionItem()
        {
            return await _context.TransactionItems.ToListAsync();
        }

        // GET: api/TransactionItem/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionItem>> GetTransactionItem(int id)
        {
            var transactionItem = await _context.TransactionItems.FindAsync(id);

            if (transactionItem == null)
            {
                return NotFound();
            }

            return transactionItem;
        }

        // PUT: api/TransactionItem/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionItem(int id, TransactionItem transactionItem)
        {
            if (id != transactionItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(transactionItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TransactionItem
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionItem>> PostTransactionItem(TransactionItem transactionItem)
        {
            _context.TransactionItems.Add(transactionItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactionItem", new { id = transactionItem.Id }, transactionItem);
        }

        // DELETE: api/TransactionItem/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionItem(int id)
        {
            var transactionItem = await _context.TransactionItems.FindAsync(id);
            if (transactionItem == null)
            {
                return NotFound();
            }

            _context.TransactionItems.Remove(transactionItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionItemExists(int id)
        {
            return _context.TransactionItem.Any(e => e.Id == id);
        }
    }
}
