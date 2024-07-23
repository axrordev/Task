using Task.Domain.Entities;

namespace TaskAdmin.Web.WebServices.Users;

public interface IUserWebService
{
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<User> GetByIdAsync(long id);
}
