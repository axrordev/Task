using Task.Service.Helpers;

namespace TaskAdmin.Web.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPathInitializer(this WebApplication app)
    {
        HttpContextHelper.ContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
        EnvironmentHelper.JwtKey = app.Configuration.GetSection("Jwt:Key").Value;
        EnvironmentHelper.TokenLifeTimeInHour = app.Configuration.GetSection("Jwt:LifeTime").Value;
        EnvironmentHelper.SmtpHost = app.Configuration.GetSection("Email:SmtpHost").Value;
        EnvironmentHelper.SmtpPort = app.Configuration.GetSection("Email:SmtpPort").Value;
        EnvironmentHelper.EmailAddress = app.Configuration.GetSection("Email:EmailAddress").Value;
        EnvironmentHelper.EmailPassword = app.Configuration.GetSection("Email:EmailPassword").Value;
    }
}
