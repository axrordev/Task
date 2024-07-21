using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

using Task.Data.DataContexts;
using Task.Domain.Entities;
using Task.Service.Exceptions;
using Task.Service.Helpers;

namespace Task.Service.Services.Accounts;

public class AccountService(AppDbContext context, IMemoryCache memoryCache) : IAccountService
{
    public async ValueTask RegisterAsync(User user)
	{
		var existUser = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
		
		if (existUser?.Email is not null)
		{
			throw new AlreadyExistException($"This user is already exist with this email | Email={user.Email}");
		}
		
		user.Password = PasswordHasher.Hash(user.Password);
		var json = JsonConvert.SerializeObject(user);
		CacheSet($"registerKey-{user.Email}", json);

		await EmailHelper.SendCodeAsync(memoryCache, user.Email, $"registerCodeKey-{user.Email}");
	}

	public void RegisterVerify(string email, string code)
	{
		var codeInCache = memoryCache.Get($"registerCodeKey-{email}");
		if (codeInCache?.ToString() != code)
			throw new ArgumentIsNotValidException("Invalid code");

		CacheSet($"verifiedAccount-{email}", "verified");

        return;
	}

	public async ValueTask<User> CreateAsync(string email)
	{
		var cacheValue = memoryCache.Get($"verifiedAccount-{email}");
		if (cacheValue is null)
			throw new ArgumentIsNotValidException("Account is not verified");

		var json = memoryCache.Get($"registerKey-{email}");
		var user = JsonConvert.DeserializeObject<User>(json.ToString());

		var createdUser = (await context.Users.AddAsync(user)).Entity;
		await context.SaveChangesAsync();

		return createdUser;
	}

	public async ValueTask<(User user, string token)> LoginAsync(string email, string password)
	{
		var existUser = await context.Users
			.FirstOrDefaultAsync(user => user.Email == email)
				?? throw new ForbiddenException("Email or Password is invalid");

        existUser.LastLoginTime = DateTime.UtcNow;
        await context.SaveChangesAsync();

        if (!PasswordHasher.Verify(password, existUser.Password))
			throw new ForbiddenException("Email or Password is invalid");

		return (user: existUser, token: AuthHelper.GenerateToken(existUser.Id, existUser.Email));
	}

	private void CacheSet(string key, string value)
	{
		var cacheOptions = new MemoryCacheEntryOptions()
				.SetSlidingExpiration(TimeSpan.FromSeconds(60))
				.SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
				.SetPriority(CacheItemPriority.Normal)
				.SetSize(1024);

		memoryCache.Set(key, value, cacheOptions);
	}
}
