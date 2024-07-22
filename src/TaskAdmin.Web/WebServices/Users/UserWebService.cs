using Task.Service.Services.Users;

namespace TaskAdmin.Web.WebServices.Users;

public class UserWebService(IUserService userService) : IUserWebService
{
    public async ValueTask<bool> DeleteAsync(long id)
    {
        return await userService.DeleteAsync(id);
    }
}
