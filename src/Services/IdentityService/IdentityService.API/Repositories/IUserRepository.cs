public interface IUserRepository
{
    Task<ApplicationUser> GetByEmailAsync(string email);
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<ApplicationUser> GetByIdAsync(string id);
    Task UpdateUserAsync(ApplicationUser user);
}