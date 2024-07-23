using AutoMapper;
using System.Numerics;
using Task.Domain.Entities;
using Task.Service.Services.Accounts;
using Task.WebApi.Models.Users;

namespace Task.WebApi.ApiServices.Accounts;

public class AccountApiService(IMapper mapper, IAccountService accountService) : IAccountApiService
{
    public async ValueTask RegisterAsync(UserRegisterModel registerModel)
    {
        await accountService.RegisterAsync(mapper.Map<User>(registerModel));
    }

    public async ValueTask RegisterVerifyAsync(string email, string code)
    {
         await accountService.RegisterVerifyAsync(email, code);
    }

    public async ValueTask<UserViewModel> CreateAsync(string email)
    {
        var result = await accountService.CreateAsync(email);
        return mapper.Map<UserViewModel>(result);
    }

    public async ValueTask<LoginViewModel> LoginAsync(string email, string password)
    {
        var result = await accountService.LoginAsync(email, password);
        var mappedResult = mapper.Map<LoginViewModel>(result.user);
        mappedResult.Token = result.token;
        return mappedResult;
    }
}
