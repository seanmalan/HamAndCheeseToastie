﻿namespace HamAndCheeseToastie.DTOs
{
    public class CreateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? Passcode { get; set; }
        public int Role { get; set; }
    }
}
