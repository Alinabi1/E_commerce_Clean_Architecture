using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Entities;

namespace UserService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task RemoveUserAsync(int id);
        Task UpdateUserAsync(User user);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<List<User>> GetUserByFirstNameAsync(string firstName);
        Task<List<User>> GetUserByLastNameAsync(string lastName);
        Task<List<User>> GetUserByRoleAsync(string role);
    }
}
