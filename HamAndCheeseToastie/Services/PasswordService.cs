using System;
using System.Threading.Tasks;
using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.Helpers;
using HamAndCheeseToastie.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HamAndCheeseToastie.Services;

public class PasswordService
{
    private readonly DatabaseContext _databaseContext;
    private readonly EmailHelper _emailHelper;

    public PasswordService(DatabaseContext context, EmailHelper emailHelper)
    {
        _databaseContext = context;
        _emailHelper = emailHelper;
    }

    public async Task<bool> ChangePasswordAsync(string username, string oldPassword, string newPassword)
    {
        var user = await _databaseContext.Users.SingleOrDefaultAsync(u => u.username == username);
        if (user == null)
            throw new Exception("User not found.");

        var hasher = new PasswordHasher<User>();
        var verificationResult = hasher.VerifyHashedPassword(user, user.password_hash, oldPassword);

        if (verificationResult == PasswordVerificationResult.Failed)
            throw new Exception("Old password is incorrect.");

        user.password_hash = hasher.HashPassword(user, newPassword);
        user.updated_at = DateTime.UtcNow;

        await _databaseContext.SaveChangesAsync();
        return true;
    }

    public async Task SendForgotPasswordEmailAsync(string email)
    {
        var user = await _databaseContext.Users.SingleOrDefaultAsync(u => u.email == email);
        if (user == null)
            throw new Exception("Email not found.");

        var resetToken = Guid.NewGuid().ToString(); // Simplified; use a more secure token in production.
        user.PasswordResetToken = resetToken;
        user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);

        await _databaseContext.SaveChangesAsync();

        var resetLink = $"https://yourapp.com/reset-password?token={resetToken}";
        var emailBody = $"Click the link to reset your password: {resetLink}";

        await _emailHelper.SendEmailAsync(email, "Password Reset Request", emailBody);
    }

    public async Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        var user = await _databaseContext.Users.SingleOrDefaultAsync(u => u.PasswordResetToken == token);
        if (user == null || user.PasswordResetTokenExpires < DateTime.UtcNow)
            throw new Exception("Invalid or expired reset token.");

        var hasher = new PasswordHasher<User>();
        user.password_hash = hasher.HashPassword(user, newPassword);
        user.PasswordResetToken = null; // Invalidate token
        user.PasswordResetTokenExpires = null;
        user.updated_at = DateTime.UtcNow;

        await _databaseContext.SaveChangesAsync();
        return true;
    }
}