using TaskAdmin.Web.Models.Accounts;
using TaskAdmin.Web.Models.Users;

namespace TaskAdmin.Web.WebServices.Accounts;

public interface IAccountWebService
{
    ValueTask RegisterAsync(RegisterModel registerModel);
    ValueTask<LoginViewModel> RegisterVerifyAsync(string email, string code);
    ValueTask<LoginViewModel> LoginAsync(LoginModel model);
}