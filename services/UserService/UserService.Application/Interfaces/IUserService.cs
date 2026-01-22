using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(string firstName, string lastName, string email, string passwordHash, string role);
        Task RemoveUserAsync(int id);
        Task ChangeFirstNameAsync(int id, string newFirstNme);
        Task ChangeLastNameAsync(int id, string newLastNme);
        Task ChangePasswordAsync(int id, string newPassword);
        Task ChangeEmailAsync(int id, string newEmail);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetUserByFirstNameAsync(string firstName);
        Task<List<User>> GetUserByLastNameAsync(string lastName);
        Task <List<User>> GetUserByRoleAsync(string role);
    }
}
