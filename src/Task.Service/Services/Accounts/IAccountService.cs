using Task.Domain.Entities;

namespace Task.Service.Services.Accounts;

public interface IAccountService
{
    ValueTask RegisterAsync(User user);
    ValueTask<(User user, string token)> RegisterVerifyAsync(string email, string code);
    ValueTask<User> CreateAsync(string email);
    ValueTask<(User user, string token)> LoginAsync(string email, string password);
}
