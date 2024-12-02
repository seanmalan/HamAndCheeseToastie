namespace HamAndCheeseToastie.DTOs
{
    public class UserDetailedDto : UserDto
    {
        public DateTime? UpdatedAt { get; set; }
        public string? Passcode { get; set; }
    }
}
