namespace HamAndCheeseToastie.DTOs
{
    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int Role { get; set; }
        public string? Passcode { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
