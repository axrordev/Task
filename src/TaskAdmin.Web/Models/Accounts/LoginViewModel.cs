namespace TaskAdmin.Web.Models.Accounts;

public class LoginViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public bool IsBlocked { get; set; }

}