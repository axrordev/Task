using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task.Service.Exceptions;
using TaskAdmin.Web.Models.Accounts;
using TaskAdmin.Web.WebServices.Accounts;

namespace TaskAdmin.Web.Controllers;

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

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("Password", "Password is required.");
                    return View(model);
                }

                await accountWebService.RegisterAsync(model);
                return RedirectToAction("RegisterVerify", new { email = model.Email });
            }
            return View(model);
        }
        catch (AlreadyExistException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message); 
            return View(model); 
        }
        catch (ForbiddenException ex)
        {
            ViewBag.Exception = ex.Message; 
        }

        return View(model);
    }


    [HttpGet("register-verify")]
    public IActionResult RegisterVerifyAsync()
    {
        if (HttpContext.User.Identity.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        return View();
    }

    [HttpPost("register-verify")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterVerify(RegisterVerifyModel model)
    {       
        try
        {
            var result = await accountWebService.RegisterVerifyAsync(model.Email, model.Code);
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
