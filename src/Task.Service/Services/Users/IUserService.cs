namespace Task.Service.Services.Users;

public interface IUserService
{
    ValueTask<bool> DeleteAsync(long id);
}

