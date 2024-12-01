using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.DTOs;
using HamAndCheeseToastie.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(DatabaseContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Fetching all users");
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.id == id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }



        [HttpGet("{id}/Transactions")]
        public async Task<IActionResult> GetUserTransactions(int id)
        {
            // First verify if there are any transactions for this user
            var hasTransactions = await _context.Transaction
                .AnyAsync(t => t.UserId == id);

            if (!hasTransactions)
            {
                return NotFound(new { Message = $"No transactions found for user with ID {id}." });
            }

            // Get all transactions for the user with customer information
            var transactions = await _context.Transaction
                .Where(t => t.UserId == id)
                .Join(
                    _context.Customer,
                    t => t.CustomerId,
                    c => c.CustomerId,
                    (t, c) => new TransactionDto
                    {
                        TransactionId = t.TransactionId,
                        TransactionDate = t.TransactionDate,
                        TotalAmount = t.TotalAmount,
                        Discount = t.Discount,
                        TaxAmount = t.TaxAmount,
                        UserId = t.UserId,
                        CustomerId = c.CustomerId,
                        CustomerName = $"{c.FirstName} {c.LastName}".Trim(),
                        PaymentMethod = t.PaymentMethod.ToString(),
                        TransactionItems = new List<TransactionItemDto>() // Initialize empty list
                    })
                .ToListAsync();

            if (!transactions.Any())
            {
                return NotFound(new { Message = $"No transactions with customer information found for user with ID {id}." });
            }

            // Get all transaction IDs for this user
            var transactionIds = transactions.Select(t => t.TransactionId).ToList();

            // Get all related transaction items with their products in a single query
            var transactionItems = await _context.TransactionItem
        .Where(ti => transactionIds.Contains(ti.TransactionId))
        .Join(
            _context.Products,
            ti => ti.ProductId,
            p => p.ID,
            (ti, p) => new
            {
                TransactionId = ti.TransactionId,
                Item = new TransactionItemDto
                {
                    Id = ti.Id,
                    TransactionId = ti.TransactionId,
                    ProductId = ti.ProductId,
                    Quantity = ti.Quantity,
                    UnitPrice = ti.UnitPrice,
                    TotalPrice = ti.TotalPrice,
                    Product = new ProductDto
                    {
                        ID = p.ID,
                        Name = p.Name,
                        BrandName = p.BrandName,
                        Weight = p.Weight,
                        CategoryId = p.CategoryId,
                        CategoryName = p.CategoryName,
                        CurrentStockLevel = p.CurrentStockLevel,
                        MinimumStockLevel = p.MinimumStockLevel,
                        Price = p.Price,
                        WholesalePrice = p.WholesalePrice,
                        EAN13Barcode = p.EAN13Barcode,
                        ImagePath = p.ImagePath
                    }
                }
            })
        .ToListAsync();

            // Group transaction items by transaction ID and add them to corresponding transactions
            foreach (var transaction in transactions)
            {
                transaction.TransactionItems = transactionItems
                    .Where(ti => ti.TransactionId == transaction.TransactionId)
                    .Select(ti => ti.Item)
                    .ToList();
            }

            return Ok(transactions);
        }







        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user == null)
            {
                _logger.LogWarning("User data is required for creation");
                return BadRequest(new { message = "User data is required" });
            }

            // Hash the password sent from the frontend
            if (!string.IsNullOrWhiteSpace(user.password_hash))
            {
                user.password_hash = PasswordHasher.HashPassword(user.password_hash);
            }
            else
            {
                _logger.LogWarning("Password is required");
                return BadRequest(new { message = "Password is required" });
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new user with ID {UserId}", user.id);
            return CreatedAtAction(nameof(Get), new { id = user.id }, user);
        }



        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            if (user == null)
            {
                _logger.LogWarning("User data is required for update");
                return BadRequest(new { message = "User data is required" });
            }

            var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.id == id);
            if (userToUpdate == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for update", id);
                return NotFound(new { message = "User not found" });
            }

            userToUpdate.username = user.username;
            userToUpdate.email = user.email;
            userToUpdate.Role = user.Role;
            userToUpdate.updated_at = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated user with ID {UserId}", id);
            return Ok(userToUpdate);
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.id == id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found for deletion", id);
                return NotFound(new { message = "User not found" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted user with ID {UserId}", id);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}
