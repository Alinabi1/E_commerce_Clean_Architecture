using System.Data;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Services
{
    public class UserServiceImp : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserServiceImp(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddUserAsync(string firstName, string lastName, string email, string passwordHash, string role)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            await _userRepository.AddUserAsync(new User(firstName, lastName, email, passwordHash, role));
        }

        public async Task RemoveUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            await _userRepository.RemoveUserAsync(id);
        }

        public async Task ChangeFirstNameAsync(int id, string newFirstName)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            if (string.Equals(user.FirstName, newFirstName, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("The new first name is the same as the current one.");
            }
            user.SetFirstName(newFirstName);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task ChangeLastNameAsync(int id, string newLastName)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            if (string.Equals(user.LastName, newLastName, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("The new last name is the same as the current one.");
            }
            user.SetLastName(newLastName);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task ChangePasswordAsync(int id, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            if (string.Equals(user.PasswordHash, newPassword, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("The new password is the same as the current one.");
            }
            user.SetPasswordHash(newPassword);
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);

        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }

        public async Task<List<User>> GetUserByFirstNameAsync(string firstName)
        {
            return await _userRepository.GetUserByFirstNameAsync(firstName);
        }

        public async Task<List<User>> GetUserByLastNameAsync(string lastName)
        {
            return await _userRepository.GetUserByLastNameAsync(lastName);
        }

        public async Task<List<User>> GetUserByRoleAsync(string role)
        {
            return await _userRepository.GetUserByRoleAsync(role);

        }
    }
}
