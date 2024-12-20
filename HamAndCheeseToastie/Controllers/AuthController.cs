﻿using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Models;
using HamAndCheeseToastie.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

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
        private readonly EmailService _emailService;
        private readonly IRsaService _rsaService;

        public AuthController(
        DatabaseContext context,
        TokenCacheService tokenCacheService,
        TokenService tokenService,
        EmailService emailService,
        IRsaService rsaService)
        {
            _context = context;
            _tokenCacheService = tokenCacheService;
            _tokenService = tokenService;
            _emailService = emailService;
            _rsaService = rsaService;
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

            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.username == request.Username || u.email == request.Email);

            if (existingUser != null)
            {
                return Conflict(new { message = "Username or Email already exists." });
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


        #region Login

        [HttpGet("public-key")]
        public IActionResult GetPublicKey()
        {
            return Ok(new { publicKey = _rsaService.GetPublicKey() });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                string decryptedPassword = _rsaService.DecryptPassword(request.Password);

                var user = await _context.Users.SingleOrDefaultAsync(u => u.email == request.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password." });
                }

                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(user, user.password_hash, decryptedPassword);
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
                    Email = user.email,
                    Role = user.Role,
                    Id = user.id
                });
            }
            catch (CryptographicException ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"Decryption error: {ex.Message}");
                return BadRequest(new { message = "Invalid encrypted data" });
            }
        }


        #endregion

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
        
         /// <summary>
        /// Changes the password for the authenticated user.
        /// </summary>
        /// <param name="request">The change password request.</param>
        /// <returns>A success message if the password is updated.</returns>
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var username = User.Identity?.Name; // Assumes user authentication
            if (username == null)
            {
                return Unauthorized(new { message = "User is not authenticated." });
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.username == username);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var hasher = new PasswordHasher<User>();
            var verificationResult = hasher.VerifyHashedPassword(user, user.password_hash, request.OldPassword);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest(new { message = "Old password is incorrect." });
            }

            user.password_hash = hasher.HashPassword(user, request.NewPassword);
            user.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully." });
        }

        /// <summary>
        /// Sends a password reset email to the user.
        /// </summary>
        /// <param name="request">The forgot password request.</param>
        /// <returns>A success message if the email is sent.</returns>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.email == request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Email not found." });
            }

            var resetToken = _tokenService.GenerateResetToken(); // Generate a reset token
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);

            await _context.SaveChangesAsync();

            var resetLink = $"https://yourapp.com/reset-password?token={resetToken}";
            var emailBody = $"Click the link to reset your password: {resetLink}";

            await _emailService.SendEmailAsync(user.email, "Password Reset Request", emailBody);

            return Ok(new { message = "Password reset email sent." });
        }

        /// <summary>
        /// Resets the password for a user using a token.
        /// </summary>
        /// <param name="request">The reset password request.</param>
        /// <returns>A success message if the password is reset.</returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.PasswordResetTokenExpires < DateTime.UtcNow)
            {
                return BadRequest(new { message = "Invalid or expired reset token." });
            }

            var hasher = new PasswordHasher<User>();
            user.password_hash = hasher.HashPassword(user, request.NewPassword);
            user.PasswordResetToken = null; // Invalidate the token
            user.PasswordResetTokenExpires = null;
            user.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Password reset successfully." });
        }
    }
}
