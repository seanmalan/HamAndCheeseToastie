// For your API project
using HamAndCheeseToastie.DTOs;
using HamAndCheeseToastie.Models;

namespace HamAndCheeseToastie.Extensions
{
    public static class UserMappingExtensions
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.id,
                UserId = user.id, // Using id as UserId since API doesn't have separate UserId
                FirstName = GetFirstName(user.username),
                LastName = GetLastName(user.username),
                Email = user.email,
                Passcode = user.Passcode,
                Role = user.Role,
                CreatedAt = user.created_at,
                last_login_at = user.updated_at,
                // Default values for MAUI-specific fields
                ItemsScanned = 0,
                CustomersAdded = 0,
                TransactionsClosed = 0
            };
        }

        private static string? GetFirstName(string fullName)
        {
            var parts = fullName.Split(' ');
            return parts.Length > 0 ? parts[0] : null;
        }

        private static string? GetLastName(string fullName)
        {
            var parts = fullName.Split(' ');
            return parts.Length > 1 ? string.Join(" ", parts.Skip(1)) : null;
        }

        public static User ToEntity(this CreateUserDto dto)
        {
            return new User
            {
                username = $"{dto.FirstName} {dto.LastName}".Trim(),
                email = dto.Email ?? "",
                password_hash = dto.Password,
                Passcode = dto.Passcode,
                Role = dto.Role,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                last_login_at = null
            };
        }
    }
}