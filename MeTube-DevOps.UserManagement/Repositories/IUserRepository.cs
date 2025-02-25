using System;
using MeTube_DevOps.UserManagement.Entities;

namespace MeTube_DevOps.UserManagement.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UsernameExistsAsync(string username);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<int?> GetUserIdByEmailAsync(string email);
    //Task<bool> ChangeUserRoleAsync(int userId, string newRole);
    Task AddUserAsync(User user);
    void UpdateUser(User user);
    Task DeleteUser(User user);
    // Task DeleteUserAsync(int userId);
}