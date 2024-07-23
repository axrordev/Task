using Task.WebApi.Models.Users;

namespace Task.WebApi.ApiServices.Accounts;

public interface IAccountApiService
{
    ValueTask RegisterAsync(UserRegisterModel registerModel);
    ValueTask RegisterVerifyAsync(string email, string code);
    ValueTask<UserViewModel> CreateAsync(string email);
    ValueTask<LoginViewModel> LoginAsync(string email, string password);
}
