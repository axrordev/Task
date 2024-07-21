using Microsoft.EntityFrameworkCore;
using Task.Data.DataContexts;
using Task.Service.Services.Accounts;
using Task.WebApi.ApiServices.Accounts;
using Task.WebApi.Extensions;
using Task.WebApi.MapperConfigurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AppDbContext>
    (option => option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddExceptionHandlers();
builder.Services.AddProblemDetails();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMemoryCache();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IAccountApiService, AccountApiService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureSwagger();

builder.Services.AddExceptions();

builder.Services.AddJwt(builder.Configuration);

builder.Services.AddProblemDetails();

builder.Services.AddHttpContextAccessor();

//builder.Services.AddServices();
var app = builder.Build();


app.AddPathInitializer();

app.UseSwagger();

app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


