using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task.Data.DataContexts;
using TaskAdmin.Web.Models;
using TaskAdmin.Web.Models.Users;
using TaskAdmin.Web.WebServices.Users;


namespace TaskAdmin.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext context;
        private readonly IUserWebService userWebService;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IUserWebService userWebService)
        {
            _logger = logger;
            this.context = context;
            this.userWebService = userWebService;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(List<long> selectedUserIds)
        {
            foreach (var id in selectedUserIds)
            {
                await userWebService.DeleteAsync(id);

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var userIdClaim = HttpContext.User.FindFirst("Id")?.Value;

                    if (userIdClaim != null && long.TryParse(userIdClaim, out long userId))
                    {
                        var user = await userWebService.GetByIdAsync(userId);

                        if (user != null && user.IsDeleted)
                        {
                            // Log out the user by clearing the cookie
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            HttpContext.Response.Redirect("/Accounts/Login");
                            
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var users = context.Users
                                .Select(u => new UserViewModel
                                {
                                    Id = u.Id,
                                    Name = u.Name,
                                    Email = u.Email,
                                    LastLoginTime = u.LastLoginTime,
                                    IsBlocked = u.IsBlocked,

                                })
                                .ToList();

            return View(users);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
