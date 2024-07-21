using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Task.Service.Helpers;
using Task.WebApi.Middlewares;

namespace Task.WebApi.Extensions;

public static class ServicesCollectionExtension
{
    public static void AddExceptions(this IServiceCollection services)
    {
        services.AddExceptionHandler<NotFoundExceptionMiddleware>();
        services.AddExceptionHandler<ForbiddenExceptionMiddleware>();
        services.AddExceptionHandler<AlreadyExistExceptionMiddleware>();
        services.AddExceptionHandler<InternalServerExceptionMiddleware>();
        services.AddExceptionHandler<ArgumentIsNotValidExceptionMiddleware>();
    }

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

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup =>
        { 
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
        });
    }
}
