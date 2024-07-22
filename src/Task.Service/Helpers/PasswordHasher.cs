
namespace Task.Service.Helpers;

public static class PasswordHasher
{
	public static string Hash(string password)
	{
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException(nameof(password), "Password cannot be null or empty.");
        }

        var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

	public static bool Verify(string password, string passwordHash)
	{
		return BCrypt.Net.BCrypt.Verify(password, passwordHash);
	}
}
