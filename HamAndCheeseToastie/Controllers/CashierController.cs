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
    public class CashierController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CashierController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Cashier
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cashier>>> GetCashier()
        {
            return await _context.Cashier.ToListAsync();
        }

        // GET: api/Cashier/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cashier>> GetCashier(int id)
        {
            var cashier = await _context.Cashier.FindAsync(id);

            if (cashier == null)
            {
                return NotFound();
            }

            return cashier;
        }

        // PUT: api/Cashier/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCashier(int id, Cashier cashier)
        {
            if (id != cashier.CashierId)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cashier
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cashier>> PostCashier(Cashier cashier)
        {
            _context.Cashier.Add(cashier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCashier", new { id = cashier.CashierId }, cashier);
        }

        // DELETE: api/Cashier/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCashier(int id)
        {
            var cashier = await _context.Cashier.FindAsync(id);
            if (cashier == null)
            {
                return NotFound();
            }

            _context.Cashier.Remove(cashier);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CashierExists(int id)
        {
            return _context.Cashier.Any(e => e.CashierId == id);
        }
    }
}
