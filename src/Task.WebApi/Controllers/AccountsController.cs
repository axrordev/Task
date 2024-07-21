using Microsoft.AspNetCore.Mvc;
using Task.WebApi.ApiServices.Accounts;
using Task.WebApi.Models.Commons;
using Task.WebApi.Models.Users;

namespace Task.WebApi.Controllers
{
    public class AccountsController(IAccountApiService accountApiService) : Controller
    {
        [HttpPost("register")]
        public async ValueTask<IActionResult> RegisterAsync(UserRegisterModel registerModel)
        {
            await accountApiService.RegisterAsync(registerModel);
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
            });
        }

        [HttpGet("register-verify")]
        public  IActionResult RegisterVerify(string email, string code)
        {
            accountApiService.RegisterVerify(email, code);
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
            });
        }

        [HttpPost("create")]
        public async ValueTask<IActionResult> CreateAsync(string email)
        {
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await accountApiService.CreateAsync(email)
            });
        }

        [HttpPost("login")]
        public async ValueTask<IActionResult> LoginAsync(string email, string password)
        {
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = await accountApiService.LoginAsync(email, password)
            });
        }

    }
}
