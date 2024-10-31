using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HamAndCheeseToastie.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }


    }
}
