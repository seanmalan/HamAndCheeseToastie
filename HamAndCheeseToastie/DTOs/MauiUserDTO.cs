namespace HamAndCheeseToastie.DTOs
{
    public class MauiUserDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Passcode { get; set; }
        public int Role { get; set; }
        public int ItemsScanned { get; set; }
        public int CustomersAdded { get; set; }
        public int TransactionsClosed { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? last_login_at { get; set; }
        public int RoleId { get; internal set; }
        public object RoleName { get; internal set; }
    }
}
