using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace TestUser.Fakes
{
    public class MockUserRepository : IUserRepository
    {
        private readonly List<User> _users = new()
        {
            new User("Jhon", "Wick", "jhon.wick@gmail.com", "jhonwick123", "Owner") { Id = 1 },
            new User("Spider", "Man", "spider.man@gmail.com", "spiderman123", "Customer") { Id = 2 },
            new User("Bat", "Man", "bat.man@gmail.com", "batman123", "Customer") { Id = 3 },
            new User("Jhon", "Shooter", "jhon.shooter@gmail.com", "jhonshooter123", "Owner") { Id = 4 }
        };

        public Task AddUserAsync(User user) {
            var nextId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task RemoveUserAsync(User user) {
            _users.Remove(user);
            return Task.CompletedTask;
        }

        public Task UpdateUserAsync(User user)
        {             
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                _users.Remove(existingUser);
                _users.Add(user);
            }
            return Task.CompletedTask;
        }

        public Task<List<User>> GetAllUsersAsync() {
            return Task.FromResult(_users.ToList());
        }

        public Task<User?> GetUserByIdAsync(int id) {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task<User?> GetUserByEmailAsync(string email) {
            var user = _users.FirstOrDefault(u => u.Email == email);
            return Task.FromResult(user);
        }

        public Task<List<User>> GetUserByFirstNameAsync(string firstName) {
            var users = _users.Where(u => u.FirstName == firstName).ToList();
            return Task.FromResult(users);
        }

        public Task<List<User>> GetUserByLastNameAsync(string lastName) {
            var users = _users.Where(u => u.LastName == lastName).ToList();
            return Task.FromResult(users);
        }

        public Task<List<User>> GetUserByRoleAsync(string role) {
            var users = _users.Where(u => u.Role.ToString() == role).ToList();
            return Task.FromResult(users);

        }

    }
}
