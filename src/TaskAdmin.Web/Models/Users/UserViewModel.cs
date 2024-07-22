namespace TaskAdmin.Web.Models.Users;

public class UserViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public bool IsBlocked { get; set; }
}
