using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUserWebService userWebService;
        private readonly AppDbContext context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context, IUserWebService userWebService)
        {
            _logger = logger;
            this.userWebService = userWebService;
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(long[] selectedUserIds)
        {
            foreach (var userId in selectedUserIds)
            {
                var user = await userWebService.GetByIdAsync(userId);
                if (user != null)
                {
                    await userWebService.DeleteAsync(userId);
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
