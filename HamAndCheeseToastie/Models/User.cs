using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamAndCheeseToastie.Models
{
    public class User
    {

        [Key]
        public int id { get; set; }
        public string username { get; set; }
        //public string password { get; set; }
        public string email { get; set; }
        public string password_hash { get; set; } = string.Empty;

        public DateTime created_at { get; set; } = DateTime.UtcNow; // Set to UTC now

        public DateTime updated_at { get; set; } = DateTime.UtcNow;

        public int role { get; set; }

    }
}
