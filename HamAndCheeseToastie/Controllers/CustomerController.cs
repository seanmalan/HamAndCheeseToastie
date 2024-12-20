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
    [Authorize]
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
        /// <summary>
        /// Retrieves a list of all customers.
        /// </summary>
        /// <response code="200">Returns the list of customers.</response>
        /// <response code="404">If no customers are found.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomer()
        {
            var customers = await _context.Customer.ToListAsync();
            if (customers == null || !customers.Any())
            {
                return NotFound("No customers found.");
            }
            return Ok(customers);
        }



        // GET: api/Customer/5
        /// <summary>
        /// Retrieves a specific customer by their ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <response code="200">Returns the customer.</response>
        /// <response code="404">If the customer is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);

            if (customer == null)
            {
                return NotFound(new { Message = "Customer not found" });
            }

            return Ok(customer);
        }



        // GET: api/Customer/search/{name}
        /// <summary>
        /// Searches for customers by name (first name or last name).
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <response code="200">Returns the matching customers.</response>
        /// <response code="404">If no customers are found.</response>
        [HttpGet("search/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Customer>>> SearchCustomersByName(string name)
        {
            var customers = await _context.Customer
                .Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name))
                .ToListAsync();

            if (!customers.Any())
            {
                return NotFound(new { Message = "No customers found matching the search criteria" });
            }

            return Ok(customers);
        }



        // GET: api/Customer/5/transactions
        /// <summary>
        /// Retrieves a specific customer's transactions by their ID.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <response code="200">Returns the customer's transactions.</response>
        /// <response code="404">If the customer is not found or has no transactions.</response>
        [HttpGet("{id}/transactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCustomerTransactions(int id)
        {
            var transactions = await (from t in _context.Transaction
                                      join u in _context.Users on t.UserId equals u.id
                                      join c in _context.Customer on t.CustomerId equals c.CustomerId
                                      where t.CustomerId == id
                                      select new
                                      {
                                          t.TransactionId,
                                          t.TransactionDate,
                                          t.TotalAmount,
                                          t.Discount,
                                          t.PaymentMethod,
                                          t.TaxAmount,
                                          t.UserId,
                                          CashierName = u.username,
                                          Customer = c
                                      })
                                    .ToListAsync();

            if (!transactions.Any())
            {
                var customer = await _context.Customer
                    .FirstOrDefaultAsync(c => c.CustomerId == id);
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

        // PUT: api/Customer/5
        /// <summary>
        /// Updates a customer's details.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <param name="customer">The updated customer object.</param>
        /// <response code="204">If the customer is updated successfully.</response>
        /// <response code="400">If the ID does not match the customer ID.</response>
        /// <response code="404">If the customer is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer data is missing.");
            }

            if (id != customer.CustomerId)
            {
                return BadRequest("Customer ID mismatch.");
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
                    return NotFound(new { Message = "Customer not found" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Customer
        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customer">The customer object to create.</param>
        /// <response code="201">Returns the newly created customer.</response>
        /// <response code="400">If the customer data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerId }, customer);
        }

        // DELETE: api/Customer/5
        /// <summary>
        /// Deletes a specific customer by their ID.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <response code="204">If the customer is deleted successfully.</response>
        /// <response code="404">If the customer is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound(new { Message = "Customer not found" });
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Helper method to check if a customer exists
        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerId == id);
        }
    }
}
