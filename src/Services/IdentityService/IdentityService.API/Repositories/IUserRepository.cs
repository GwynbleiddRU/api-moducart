using System.Threading.Tasks;
using IdentityService.API.Models;

namespace IdentityService.API.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task<ApplicationUser> GetByIdAsync(string id);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task UpdateUserAsync(ApplicationUser user);
    }
}
