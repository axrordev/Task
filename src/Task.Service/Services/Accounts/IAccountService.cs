using Task.Domain.Entities;

namespace Task.Service.Services.Accounts;

public interface IAccountService
{
    ValueTask RegisterAsync(User user);
    void RegisterVerify(string email, string code);
    ValueTask<User> CreateAsync(string email);
    ValueTask<(User user, string token)> LoginAsync(string email, string password);
}
