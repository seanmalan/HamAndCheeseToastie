namespace HamAndCheeseToastie.Models;

public class ChangePasswordRequest
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}
