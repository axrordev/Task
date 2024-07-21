
using Task.Domain.Commons;

namespace Task.Domain.Entities;

public class User : Auditable
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public bool IsBlocked { get; set; }
}
