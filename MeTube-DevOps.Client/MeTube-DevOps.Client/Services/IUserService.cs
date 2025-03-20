﻿using MeTube_DevOps.Client.Models;
using MeTube_DevOps.Client.DTO.UserDTOs;

namespace MeTube_DevOps.Client.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(User user);
        Task<User?> LoginAsync(string username, string password);
        Task<bool> LogoutAsync();
        Task<string> GetTokenAsync(string username, string password);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<UserDetails>> GetAllUsersDetailsAsync();
        Task<int?> GetUserIdByEmailAsync(string email);
        Task<User> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<Dictionary<string, string>> IsUserAuthenticated();
        Task<Dictionary<string, string>> DoesUserExistAsync(string username, string email);

        Task<string> GetLogedInUserName();
    }
}
