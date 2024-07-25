using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Task.Data.DataContexts;
using Task.Service.Services.Accounts;
using Task.Service.Services.Users;
using TaskAdmin.Web.Extensions;
using TaskAdmin.Web.MapperConfigurations;
using TaskAdmin.Web.WebServices.Accounts;
using TaskAdmin.Web.WebServices.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMemoryCache();

// Session qo'llab-quvvatlashni qo'shing
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session muddati
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserWebService, UserWebService>();
builder.Services.AddScoped<IAccountWebService, AccountWebService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Accounts/login";
        options.AccessDeniedPath = "/access-denied";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(6);
    });

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.AddPathInitializer();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sessionni yoqing
app.UseSession();

// CheckUserStatusMiddleware qo'llang
//app.Use(async (context, next) =>
//{
//    var userService = context.RequestServices.GetRequiredService<IUserService>();
//    var middleware = new CheckUserStatusMiddleware(next, userService);
//    await middleware.InvokeAsync(context);
//});

app.UseMiddleware<CheckUserStatusMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Login}/{id?}");

app.Run();
