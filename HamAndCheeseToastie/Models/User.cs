using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    [Table("users")] // Set table name to lowercase for PostgreSQL compatibility
    public class User
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string password_hash { get; set; } = string.Empty;

        public DateTime created_at { get; set; } = DateTime.UtcNow;

        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        [Required]
        public int Role { get; set; }

        // New fields for password reset functionality
        public string? PasswordResetToken { get; set; } // Nullable since it's optional

        public DateTime? PasswordResetTokenExpires { get; set; } // Nullable since it's optional
    }
}