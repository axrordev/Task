
using AutoMapper;
using Task.Domain.Entities;
using Task.Service.Services.Accounts;
using Task.Service.Services.Users;
using TaskAdmin.Web.Models.Accounts;
using TaskAdmin.Web.Models.Users;

namespace TaskAdmin.Web.WebServices.Accounts;

public class AccountWebService(IAccountService accountService, IUserService userService, IMapper mapper) : IAccountWebService
{
    public async ValueTask RegisterAsync(RegisterModel registerModel)
    {
        await accountService.RegisterAsync(mapper.Map<User>(registerModel));
    }

    public void RegisterVerify(string email, string code)
    {
        accountService.RegisterVerify(email, code);
    }

    public async ValueTask<UserViewModel> CreateAsync(string email)
    {
        var result = await accountService.CreateAsync(email);
        return mapper.Map<UserViewModel>(result);
    }

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