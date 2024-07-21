using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task.Service.Exceptions;
using Task.Web.Models.Accounts;
using Task.Web.WebServices.Accounts;

namespace Task.Web.Controllers;

public class AccountsController(IAccountWebService accountWebService) : Controller
{
    public IActionResult Login()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost]
    public async ValueTask<IActionResult> Login(LoginModel model)
    {
        try
        {
            var result = await accountWebService.LoginAsync(model);
            if (result is not null)
            {
                var claims = new List<Claim>
                {
                   new Claim("Id", result.Id.ToString()),
                   new Claim("Name", $"{result.Name}")
                };

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity));

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
        catch (ForbiddenException ex)
        {
            ViewBag.Exception = ex.Message;
        }

        return View();
    }

    public async ValueTask<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }
}
