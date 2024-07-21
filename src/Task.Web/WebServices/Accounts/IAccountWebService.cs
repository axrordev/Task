
using Task.Web.Models.Accounts;
using Task.Web.Models.Users;

namespace Task.Web.WebServices.Accounts;

public interface IAccountWebService
{
    ValueTask<LoginViewModel> LoginAsync(LoginModel model);
}