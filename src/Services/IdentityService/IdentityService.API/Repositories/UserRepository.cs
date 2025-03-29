using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityService.API.Extensions;
using IdentityService.API.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;

namespace IdentityService.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<ApplicationUser> _users;
        private readonly IdentityService.API.Extensions.IPasswordHasher<ApplicationUser> _passwordHasher;

        public UserRepository(
            IMongoDatabase database,
            IdentityService.API.Extensions.IPasswordHasher<ApplicationUser> passwordHasher
        )
        {
            _users = database.GetCollection<ApplicationUser>("Users");
            _passwordHasher = passwordHasher;
        }

        public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password)
        {
            // Check if user already exists
            var existingUser = await GetByEmailAsync(user.Email);
            if (existingUser != null)
                throw new InvalidOperationException("User with this email already exists");

            // Hash password
            user.PasswordHash = _passwordHasher.HashPassword(user, password);
            user.CreatedAt = DateTime.UtcNow;

            // Assign default role
            user.Roles = new List<string> { "User" };

            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task UpdateUserAsync(ApplicationUser user)
        {
            var filter = Builders<ApplicationUser>.Filter.Eq(u => u.Id, user.Id);
            await _users.ReplaceOneAsync(filter, user);
        }
    }
}
