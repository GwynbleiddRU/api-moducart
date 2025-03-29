using IdentityService.API.DTOs;

namespace IdentityService.API.Services
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        Task<string> LoginAsync(LoginDto loginDto);
        Task<UserDto> GetUserProfileAsync(string userId);
    }
}