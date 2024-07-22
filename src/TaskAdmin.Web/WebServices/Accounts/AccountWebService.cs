
using AutoMapper;
using Task.Domain.Entities;
using Task.Service.Services.Accounts;
using Task.Service.Services.Users;
using TaskAdmin.Web.Models.Accounts;
using TaskAdmin.Web.Models.Users;

namespace TaskAdmin.Web.WebServices.Accounts;

public class AccountWebService(IAccountService accountService, IMapper mapper) : IAccountWebService
{
    public async ValueTask RegisterAsync(RegisterModel registerModel)
    {
        await accountService.RegisterAsync(mapper.Map<User>(registerModel));
    }

    public async ValueTask<LoginViewModel> RegisterVerifyAsync(string email, string code)
    {
        var result =  await accountService.RegisterVerifyAsync(email, code);
        return new LoginViewModel
        {
            Id = result.user.Id,
            Name = result.user.Name,
            Email = result.user.Email,
            Token = result.token,
            IsBlocked = result.user.IsBlocked,
            LastLoginTime = result.user.LastLoginTime,
        };
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
            IsBlocked = result.user.IsBlocked,
            LastLoginTime = result.user.LastLoginTime,
        };
    }
}