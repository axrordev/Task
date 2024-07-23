using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks; // Import the correct namespace for Task
using Task.Service.Services.Users;

public class CheckUserStatusMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IUserService _userService;

    public CheckUserStatusMiddleware(RequestDelegate next, IUserService userService)
    {
        _next = next;
        _userService = userService;
    }

    public async System.Threading.Tasks.Task InvokeAsync(HttpContext context) // Fully qualify Task
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var userIdClaim = context.User.FindFirst("Id")?.Value;

            if (userIdClaim != null && long.TryParse(userIdClaim, out long userId))
            {
                var user = await _userService.GetByIdAsync(userId);

                if (user != null && user.IsDeleted)
                {
                    // Log out the user by clearing the cookie
                    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    context.Response.Redirect("/Accounts/Login");
                    return;
                }
            }
        }

        await _next(context); // Ensure this is reachable and returns a Task
    }
}
