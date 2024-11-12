using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Models;
using Microsoft.AspNetCore.Authorization;
using HamAndCheeseToastie.DTOs;

namespace HamAndCheeseToastie.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CustomerController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            var products = await _context.Customer.ToListAsync();

            return Ok(products);
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }



        // GET: api/Customer/5/transactions
        [HttpGet("{id}/transactions")]
        public async Task<ActionResult> GetCustomerTransactions(int id)
        {
            
            var transactions = await _context.Transaction
                                              .Include(t => t.Customer) 
                                              .Where(t => t.CustomerId == id)
                                              .ToListAsync();

            if (!transactions.Any())
            {
                var customer = await _context.Customer
                                             .Where(c => c.CustomerId == id)
                                             .FirstOrDefaultAsync();
                if (customer == null)
                {
                    return NotFound(new { Message = "Customer not found" });
                }

                return Ok(new
                {
                    Customer = customer,
                    Transactions = new List<object>()
                });
            }
            return Ok(new
            {
                Customer = transactions.First().Customer,
                Transactions = transactions
            });
        }


        // GET: api/Customer/maui
        [HttpGet("api/maui/customers")]
        public async Task<IActionResult> GetCustomerMaui()
        {
            var products = await _context.Customer
                .Select(c => new MauiCustomerDto()
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

            return Ok(products);
        }




        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}
