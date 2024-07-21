using Microsoft.EntityFrameworkCore;
using Task.Data.DataContexts;
using Task.Domain.Entities;
using Task.Service.Exceptions;

namespace Task.Service.Services.Users;

public class UserService(AppDbContext context) : IUserService
{
    public async ValueTask<bool> DeleteAsync(long id)
    {
        var existUser = await context.Users.FirstOrDefaultAsync(user => user.Id == id)
            ?? throw new NotFoundException("User is not found");

        existUser.IsDeleted = true;
        existUser.DeletedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return true;
    }
}

