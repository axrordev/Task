using Task.Domain.Entities;

namespace Task.Service.Services.Users;

public interface IUserService
{
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<User> GetByIdAsync(long  id);
}

