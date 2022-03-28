using System.Security.Cryptography;
using System.Text;

namespace Suzan.Application.Helpers;

public static class PasswordHashHelper
{
    public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hms = new HMACSHA512(passwordSalt);
        var hash = hms.ComputeHash(Encoding.UTF8.GetBytes(password));

        return hash.SequenceEqual(passwordHash);
    }

    public static (byte[] passwordHash, byte[] passwordSalt) HashPassword(string password)
    {
        using var hms = new HMACSHA512();
        var salt = hms.Key;
        var hash = hms.ComputeHash(Encoding.UTF8.GetBytes(password));
        return (hash, salt);
    }
}