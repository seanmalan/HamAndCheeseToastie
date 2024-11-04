using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HamAndCheeseToastie.Controllers
{

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

    }



    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly TokenCacheService _tokenCacheService;

        public AuthController(DatabaseContext context, TokenCacheService tokenCacheService)
        {
            _context = context;
            _tokenCacheService = tokenCacheService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Validate that the passwords match
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            // Hash the password and create the user
            var hashedPassword = PasswordHasher.HashPassword(request.Password);
            var user = new User { username = request.Username, password_hash = hashedPassword, email = request.Email, role = '3' };
            _context.users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.users.SingleOrDefaultAsync(u => u.email == request.Email);
            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.password_hash))
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = TokenService.GenerateToken(user.id.ToString());
            _tokenCacheService.CacheToken(token, user.id.ToString());

            return Ok(new { Token = token });
        }

        [HttpPost("validate")]
        public IActionResult ValidateToken(string token)
        {
            if (!_tokenCacheService.IsTokenValid(token))
            {
                return Unauthorized("Token expired or invalid.");
            }

            var principal = TokenService.ValidateToken(token);
            if (principal == null)
            {
                return Unauthorized("Invalid token.");
            }

            return Ok("Token is valid.");
        }

        [HttpPost("logout")]
        public IActionResult Logout(string token)
        {
            _tokenCacheService.RemoveToken(token);
            return Ok("Logged out.");
        }
    }
}
