using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Models;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashierController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<CashierController> _logger;

        public CashierController(DatabaseContext context, ILogger<CashierController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all cashiers.
        /// </summary>
        /// <returns>List of Cashier objects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cashier>>> GetCashier()
        {
            _logger.LogInformation("Fetching all cashiers");
            return Ok(await _context.Cashier.ToListAsync());
        }

        /// <summary>
        /// Retrieves a specific cashier by unique ID.
        /// </summary>
        /// <param name="id">The ID of the cashier to retrieve</param>
        /// <returns>Cashier object if found, otherwise NotFound</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cashier>> GetCashier(int id)
        {
            _logger.LogInformation("Fetching cashier with ID {CashierId}", id);
            var cashier = await _context.Cashier.FindAsync(id);

            if (cashier == null)
            {
                _logger.LogWarning("Cashier with ID {CashierId} not found", id);
                return NotFound(new { message = "Cashier not found" });
            }

            return Ok(cashier);
        }

        /// <summary>
        /// Updates an existing cashier.
        /// </summary>
        /// <param name="id">The ID of the cashier to update</param>
        /// <param name="cashier">Updated cashier data</param>
        /// <returns>No content if successful, otherwise appropriate error</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCashier(int id, [FromBody] Cashier cashier)
        {
            if (id != cashier.CashierId)
            {
                _logger.LogWarning("Mismatched Cashier ID in request: {RequestId} vs {CashierId}", id, cashier.CashierId);
                return BadRequest(new { message = "Cashier ID mismatch" });
            }

            _context.Entry(cashier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CashierExists(id))
                {
                    _logger.LogWarning("Cashier with ID {CashierId} not found for update", id);
                    return NotFound(new { message = "Cashier not found" });
                }
                else
                {
                    _logger.LogError("Concurrency error while updating cashier with ID {CashierId}", id);
                    throw;
                }
            }

            _logger.LogInformation("Cashier with ID {CashierId} updated successfully", id);
            return NoContent();
        }

        /// <summary>
        /// Creates a new cashier.
        /// </summary>
        /// <param name="cashier">The cashier object to create</param>
        /// <returns>Created cashier object</returns>
        [HttpPost]
        public async Task<ActionResult<Cashier>> PostCashier([FromBody] Cashier cashier)
        {
            _context.Cashier.Add(cashier);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new cashier with ID {CashierId}", cashier.CashierId);
            return CreatedAtAction(nameof(GetCashier), new { id = cashier.CashierId }, cashier);
        }

        /// <summary>
        /// Deletes a specific cashier by unique ID.
        /// </summary>
        /// <param name="id">The ID of the cashier to delete</param>
        /// <returns>No content if successful, otherwise appropriate error</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCashier(int id)
        {
            var cashier = await _context.Cashier.FindAsync(id);
            if (cashier == null)
            {
                _logger.LogWarning("Cashier with ID {CashierId} not found for deletion", id);
                return NotFound(new { message = "Cashier not found" });
            }

            _context.Cashier.Remove(cashier);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted cashier with ID {CashierId}", id);
            return NoContent();
        }

        private bool CashierExists(int id)
        {
            return _context.Cashier.Any(e => e.CashierId == id);
        }
    }
}
