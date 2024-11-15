﻿using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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
        private readonly TokenService _tokenService;

        public AuthController(DatabaseContext context, TokenCacheService tokenCacheService, TokenService tokenService)
        {
            _context = context;
            _tokenCacheService = tokenCacheService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The registration details, including username, email, password, and confirmation password.</param>
        /// <returns>A success message if the registration is successful; otherwise, an error message.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match." });
            }

            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                username = request.Username,
                email = request.Email,
                Role = '3'
            };
            user.password_hash = hasher.HashPassword(user, request.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User registered successfully." });
        }

        /// <summary>
        /// Logs in a user and returns a token.
        /// </summary>
        /// <param name="request">The login details, including email and password.</param>
        /// <returns>A token if the login is successful; otherwise, an error message.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.email == request.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.password_hash, request.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            var token = _tokenService.GenerateToken(user.id.ToString());
            _tokenCacheService.CacheToken(token, user.id.ToString());

            return Ok(new
            {
                Token = token,
                Username = user.username,
                Email = user.email
            });
        }

        /// <summary>
        /// Validates a token to check if it is still active.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <returns>A message indicating if the token is valid or invalid.</returns>
        [HttpPost("validate")]
        public IActionResult ValidateToken([FromBody] string token)
        {
            if (!_tokenCacheService.IsTokenValid(token))
            {
                return Unauthorized(new { message = "Token expired or invalid." });
            }

            var principal = _tokenService.ValidateToken(token);
            if (principal == null)
            {
                return Unauthorized(new { message = "Invalid token." });
            }

            return Ok(new { message = "Token is valid." });
        }

        /// <summary>
        /// Logs out a user by invalidating their token.
        /// </summary>
        /// <param name="token">The token to invalidate.</param>
        /// <returns>A message indicating successful logout.</returns>
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] string token)
        {
            _tokenCacheService.RemoveToken(token);
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
