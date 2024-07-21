namespace Task.Web.WebServices.Users;

public interface IUserWebService
{
    ValueTask<bool> DeleteAsync(long id);
}
