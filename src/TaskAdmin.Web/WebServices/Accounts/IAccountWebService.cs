using TaskAdmin.Web.Models.Accounts;
using TaskAdmin.Web.Models.Users;

namespace TaskAdmin.Web.WebServices.Accounts;

public interface IAccountWebService
{
    ValueTask RegisterAsync(RegisterModel registerModel);
    void RegisterVerify(string email, string code);
    ValueTask<UserViewModel> CreateAsync(string email);
    ValueTask<LoginViewModel> LoginAsync(LoginModel model);
}