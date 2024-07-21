
using Task.Service.Services.Accounts;
using Task.Service.Services.Users;
using Task.Web.Models.Accounts;
using Task.Web.Models.Users;

namespace Task.Web.WebServices.Accounts;

public class AccountWebService(IAccountService accountService, IUserService userService) : IAccountWebService
{
    public async ValueTask<LoginViewModel> LoginAsync(LoginModel model)
    {
        var result = await accountService.LoginAsync(model.Email, model.Password);
        return new LoginViewModel
        {
            Id = result.user.Id,
            Name = result.user.Name,
            Email = result.user.Email,
            Token = result.token,  
        };
    }
}