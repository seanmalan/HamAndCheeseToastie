using HamAndCheeseToastie.Database;
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

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user == null)
            {
                _logger.LogWarning("User data is required for creation");
                return BadRequest(new { message = "User data is required" });
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
