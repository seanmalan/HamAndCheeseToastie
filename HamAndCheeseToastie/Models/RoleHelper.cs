using HamAndCheeseToastie.Database;

namespace HamAndCheeseToastie.Models
{
    public static class RoleHelper
    {
        public const string SUPERADMIN = "SUPERADMIN";
        public const string ADMIN = "ADMIN";
        public const string USER = "USER";
        public const string CASHIER = "CASHIER";

        public static bool IsAdminRole(int roleId)
        {
            return roleId == 1 || roleId == 2; // SUPERADMIN or ADMIN
        }

        public static bool IsSuperAdmin(int roleId)
        {
            return roleId == 1;
        }

        public static async Task<string> GetRoleName(DatabaseContext context, int roleId)
        {
            var role = await context.Roles.FindAsync(roleId);
            return role?.Name ?? "UNKNOWN";
        }
    }
}
