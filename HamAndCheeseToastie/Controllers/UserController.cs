using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HamAndCheeseToastie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users); // Return 200 OK with the list of users
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.id == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" }); // Return 404 if user is not found
            }

            return Ok(user); // Return 200 OK with the found user
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "User data is required" }); // Return 400 Bad Request if user is null
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Return 201 Created with the location of the new user and the user data
            return CreatedAtAction(nameof(Get), new { id = user.id }, user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest(new { message = "User data is required" }); // Return 400 Bad Request if user is null
            }

            var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.id == id);

            if (userToUpdate == null)
            {
                return NotFound(new { message = "User not found" }); // Return 404 if user is not found
            }

            // Update user fields
            userToUpdate.username = user.username;
            userToUpdate.email = user.email;
            userToUpdate.roleId = user.roleId;
            userToUpdate.updated_at = DateTime.Now;

            await _context.SaveChangesAsync();

            return Ok(userToUpdate); // Return 200 OK with the updated user
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.id == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" }); // Return 404 if user is not found
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" }); // Return 200 OK with confirmation message
        }
    }
}
