using Microsoft.AspNetCore.Identity;

namespace IdentityService.API.Extensions
{
    public interface IPasswordHasher<TUser>
        where TUser : class
    {
        string HashPassword(TUser user, string password);
        PasswordVerificationResult VerifyHashedPassword(
            TUser user,
            string hashedPassword,
            string providedPassword
        );
    }
}
