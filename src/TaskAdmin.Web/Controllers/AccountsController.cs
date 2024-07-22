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

    // GET: /Account/Register
    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            await accountWebService.RegisterAsync(model);
            return RedirectToAction("RegisterVerify");
        }
        return View(model);
    }

    // GET: /Account/RegisterVerify
    [HttpGet("register-verify")]
    public IActionResult RegisterVerify()
    {
        return View();
    }

    // POST: /Account/RegisterVerify
    [HttpPost("register-verify")]
    [ValidateAntiForgeryToken]
    public IActionResult RegisterVerify(string email, string code)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(code))
        {
            ModelState.AddModelError("", "Email and code are required.");
            return View();
        }

        accountWebService.RegisterVerify(email, code);
        return RedirectToAction("Create");
    }

    // GET: /Account/Create
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Account/Create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            ModelState.AddModelError("", "Email is required.");
            return View();
        }

        var userViewModel = await accountWebService.CreateAsync(email);
        return View(userViewModel);
    }

    public async ValueTask<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login");
    }
}
