using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;
using Task.Service.Services.Users;

public class CheckUserStatusMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CheckUserStatusMiddleware> _logger;

    public CheckUserStatusMiddleware(RequestDelegate next, ILogger<CheckUserStatusMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async System.Threading.Tasks.Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("CheckUserStatusMiddleware invoked.");

        if (context.User.Identity.IsAuthenticated)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"User ID claim: {userIdClaim}");

            if (userIdClaim != null && long.TryParse(userIdClaim, out long userId))
            {
                using (var scope = context.RequestServices.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    var user = await userService.GetByIdAsync(userId);
                    _logger.LogInformation($"User: {user}");

                    if (user == null || user.IsBlocked || user.IsDeleted)
                    {
                        _logger.LogInformation("User is either blocked or deleted. Redirecting to login page.");
                        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                        context.Response.Redirect("/Accounts/Login");
                        return;
                    }
                }
            }
        }

        await _next(context);

    }
}
