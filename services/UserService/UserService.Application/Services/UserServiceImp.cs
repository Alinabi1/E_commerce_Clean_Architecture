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

        public async Task AddUserAsync(User user)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            await _userRepository.AddUserAsync(user);
        }

        public async Task RemoveUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            await _userRepository.RemoveUserAsync(user);
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("No value to change to.");
            }

            var userFromDb = await _userRepository.GetUserByIdAsync(id);
            if (userFromDb == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            if (!string.IsNullOrEmpty(user.FirstName))
            {
                if (string.Equals(userFromDb.FirstName, user.FirstName, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException("The new first name is the same as the current one.");
                }
                userFromDb.SetFirstName(user.FirstName);
            }
            if (!string.IsNullOrEmpty(user.LastName))
            {
                if (string.Equals(userFromDb.LastName, user.LastName, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException("The new last name is the same as the current one.");
                }
                userFromDb.SetLastName(user.LastName);
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                if (string.Equals(userFromDb.Email, user.Email, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException("The new email is the same as the current one.");
                }
                var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("A user with this email already exists.");
                }
                userFromDb.SetEmail(user.Email);
            }
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                if (string.Equals(userFromDb.PasswordHash, user.PasswordHash, StringComparison.Ordinal))
                {
                    throw new InvalidOperationException("The new password is the same as the current one.");
                }
                userFromDb.SetPasswordHash(user.PasswordHash);
            }
            await _userRepository.UpdateUserAsync(userFromDb);
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
