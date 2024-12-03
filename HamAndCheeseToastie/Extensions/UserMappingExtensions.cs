using HamAndCheeseToastie.Database;
using HamAndCheeseToastie.DTOs;
using HamAndCheeseToastie.Models;

namespace HamAndCheeseToastie.Extensions
{
    public static class UserMappingExtensions
    {
        public static async Task<UserBasicDto> ToBasicDtoAsync(this User user, DatabaseContext context)
        {
            return new UserBasicDto
            {
                Id = user.id,
                Username = user.username,
                RoleId = user.Role,
                RoleName = await RoleHelper.GetRoleName(context, (char)user.Role)
            };
        }

        public static async Task<UserDto> ToDtoAsync(this User user, DatabaseContext context)
        {
            return new UserDto
            {
                Id = user.id,
                Username = user.username,
                Email = user.email,
                RoleId = user.Role,
                RoleName = await RoleHelper.GetRoleName(context, (char)user.Role),
                CreatedAt = user.created_at,
                LastLoginAt = user.last_login_at,
                FirstName = GetFirstName(user.username),
                LastName = GetLastName(user.username)
            };
        }

        public static async Task<MauiUserDTO> ToMauiDtoAsync(this User user, DatabaseContext context)
        {
            return new MauiUserDTO
            {
                Id = user.id,
                UserId = user.id,
                FirstName = GetFirstName(user.username),
                LastName = GetLastName(user.username),
                Email = user.email,
                Passcode = user.Passcode,
                RoleId = user.Role,
                RoleName = await RoleHelper.GetRoleName(context, (char)user.Role),
                CreatedAt = user.created_at,
                last_login_at = user.updated_at,
                ItemsScanned = 0,
                CustomersAdded = 0,
                TransactionsClosed = 0
            };
        }

        // Helper methods remain the same
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

        // Collection mappings become async
        public static async Task<IEnumerable<UserBasicDto>> ToBasicDtosAsync(this IEnumerable<User> users, DatabaseContext context)
        {
            var dtos = new List<UserBasicDto>();
            foreach (var user in users)
            {
                dtos.Add(await user.ToBasicDtoAsync(context));
            }
            return dtos;
        }

        public static async Task<IEnumerable<UserDto>> ToDtosAsync(this IEnumerable<User> users, DatabaseContext context)
        {
            var dtos = new List<UserDto>();
            foreach (var user in users)
            {
                dtos.Add(await user.ToDtoAsync(context));
            }
            return dtos;
        }

        public static async Task<IEnumerable<MauiUserDTO>> ToMauiDtosAsync(this IEnumerable<User> users, DatabaseContext context)
        {
            var dtos = new List<MauiUserDTO>();
            foreach (var user in users)
            {
                dtos.Add(await user.ToMauiDtoAsync(context));
            }
            return dtos;
        }
    }
}