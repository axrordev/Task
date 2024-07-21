using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task.Data.DataContexts;
using Task.Web.Models;
using Task.Web.Models.Accounts;
using Task.Web.Models.Users;
using Task.Web.WebServices.Users;


namespace Task.Web.Controllers
{
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
        public async Task<IActionResult> DeleteUsers([FromBody] List<long> userIds)
        {
            foreach (var userId in userIds)
            {
                await userWebService.DeleteAsync(userId);
            }
            return Ok();
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
