using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestUser.Fakes;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Domain.Entities;

namespace TestUser.Application
{
    public class TestUserService
    {
        private readonly IUserService _userService;

        public TestUserService()
        {
            IUserRepository mockRepository = new MockUserRepository();
            _userService = new UserServiceImp(mockRepository);
        }

        [Fact]
        public async Task AddUser_ShouldWork_AllDataValid()
        {
            string firstName = "Alice";
            string lastName = "Smith";
            string email = "alice.smith@gmail.com";
            string passwordHash = "alicesmith123";
            string role = "Customer";

            await _userService.AddUserAsync(firstName, lastName, email, passwordHash, role);

            User addedUser = await _userService.GetUserByEmailAsync(email);

            Assert.NotNull(addedUser);
            Assert.Equal(firstName, addedUser.FirstName);
            Assert.Equal(lastName, addedUser.LastName);
            Assert.Equal(email, addedUser.Email);
            Assert.Equal(passwordHash, addedUser.PasswordHash);
            Assert.Equal(role, addedUser.Role.ToString());
        }

        [Fact]
        public async Task AddUser_ShouldFail_MissingFirstName()
        {
            string firstName = null;
            string lastName = "Wick";
            string email = "jhonny.wick@gmail.com";
            string passwordHash = "jhonwick123";
            string role = "Customer";

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.AddUserAsync(firstName, lastName, email, passwordHash, role);
            });
        }

        [Fact]
        public async Task AddUser_ShouldFail_MissingLastName()
        {
            string firstName = "Jhonny";
            string lastName = null;
            string email = "jhonny.wick@gmail.com";
            string passwordHash = "jhonwick123";
            string role = "Customer";

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.AddUserAsync(firstName, lastName, email, passwordHash, role);
            });
        }

        [Fact]
        public async Task AddUser_ShouldNotWork_WrongEmail()
        {
            string firstName = "Alice";
            string lastName = "Smith";
            string email = "alice.smithgmail.com";
            string passwordHash = "alicesmith123";
            string role = "Customer";

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.AddUserAsync(firstName, lastName, email, passwordHash, role);
            });
        }

        [Fact]
        public async Task AddUser_ShouldNotWork_DuplicatedEmail()
        {
            string firstName = "Jhon";
            string lastName = "Wick";
            string email = "jhon.wick@gmail.com";
            string passwordHash = "jhonwick123";
            string role = "Customer";

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _userService.AddUserAsync(firstName, lastName, email, passwordHash, role);
            });
        }

        [Fact]
        public async Task AddUser_ShouldFail_NoPassword()
        {
            string firstName = "Alice";
            string lastName = "Smith";
            string email = "alice.smith@gmail.com";
            string passwordHash = " ";
            string role = "Customer";

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.AddUserAsync(firstName, lastName, email, passwordHash, role);
            });
        }

        [Fact]
        public async Task AddUser_ShouldFail_NoRole()
        {
            string firstName = "Jhonny";
            string lastName = "Wick";
            string email = "jhonny.wick@gmail.com";
            string passwordHash = "jhonwick123";
            string role = "";

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _userService.AddUserAsync(firstName, lastName, email, passwordHash, role);
            });
        }

        [Fact]
        public async Task RemoveUser_ShouldWork_UserExists()
        {
            int userId = 1;
            await _userService.RemoveUserAsync(userId);
            User removedUser = await _userService.GetUserByIdAsync(userId);

            Assert.Null(removedUser);
        }

        [Fact]
        public async Task RemoveUser_ShouldNotWork_NotExistingUser()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _userService.RemoveUserAsync(999);
            });

        }

        [Fact]
        public async Task ChangeFirstName_ShouldWork_AllDataValid()
        {
            string newFirstName = "Jonathan";
            int userId = 1;
            await _userService.ChangeFirstNameAsync(userId, newFirstName);
            User updatedUser = await _userService.GetUserByIdAsync(userId);
            Assert.Equal(newFirstName, updatedUser.FirstName);
        }

        [Fact]
        public async Task ChangeFirstName_ShouldNotWork_SameFirstName()
        {
            string newFirstName = "Jhon";
            int userId = 1;
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _userService.ChangeFirstNameAsync(userId, newFirstName);
            });
        }

        [Fact]
        public async Task ChangeFirstName_ShouldNotWork_NotExistingUser()
        {
            string newFirstName = "Jonathan";
            int userId = 999;
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _userService.ChangeFirstNameAsync(userId, newFirstName);
            });

        }

        [Fact]
        public async Task ChangeLastName_ShouldWork_AllDataValis()
        {
            string newLastName = "Doe";
            int userId = 2;
            await _userService.ChangeLastNameAsync(userId, newLastName);
            User updatedUser = await _userService.GetUserByIdAsync(userId);
            Assert.Equal(newLastName, updatedUser.LastName);
        }

        [Fact]
        public async Task ChangeLastName_ShouldNotWork_SameLastName()
        {
            string newLastName = "Man";
            int userId = 2;
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _userService.ChangeLastNameAsync(userId, newLastName);
            }); 
        }

        [Fact]
        public async Task ChangeLastName_ShouldNotWork_NotExistingUser()
        {
            string newLastName = "Doe";
            int userId = 999;
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _userService.ChangeLastNameAsync(userId, newLastName);
            });
        }

        [Fact]
        public async Task ChangePassword_ShouldWork_AllDataValid()
        { 
            int userId = 3;
            string newPasswordHash = "newpassword123";
            await _userService.ChangePasswordAsync(userId, newPasswordHash);
            User updatedUser = await _userService.GetUserByIdAsync(userId);
            Assert.Equal(newPasswordHash, updatedUser.PasswordHash);
        }

        [Fact]
        public async Task ChangePassword_ShouldNotWork_SamePassWord()
        {
            int userId = 3;
            string newPasswordHash = "batman123";
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _userService.ChangePasswordAsync(userId, newPasswordHash);
            });
        }

        [Fact]
        public async Task ChangePassword_ShouldNotWork_NotExistingUser()
        {
            int userId = 999;
            string newPasswordHash = "newpassword123";
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _userService.ChangePasswordAsync(userId, newPasswordHash);
            });
        }

        [Fact]
        public async Task ChangeEmailAsync_shouldWork_AllDataValid()
        {
            int userId = 1;
            string newEmail = "jhon.wick1@gmail.com";
            await _userService.ChangeEmailAsync(userId, newEmail);
            var updatedUser = await _userService.GetUserByIdAsync(userId);
            Assert.NotNull(updatedUser);
            Assert.Equal(newEmail, updatedUser.Email);
        }

        [Fact]
        public async Task ChangeEmail_ShouldNotWork_SameEmail()
        {
            int userId = 1;
            string newEmail = "jhon.wick@gmail.com";
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _userService.ChangeEmailAsync(userId, newEmail);
            });
        }

        [Fact]
        public async Task ChangeEmail_ShouldNotWork_NotExistingUser()
        {
            int userId = 999;
            string newEmail = "jhon.wick1@gmail.com";
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await _userService.ChangeEmailAsync(userId, newEmail);
            });

        }

        [Fact]
        public async Task GetAllUsers_ShouldWork_ReturnAllUsers()
        {
            List<User> users = await _userService.GetAllUsersAsync();
            Assert.Equal(4, users.Count);
        }

        [Fact]
        public async Task GetUserById_ShouldWork_ExistingUser()
        { 
            User user = await _userService.GetUserByIdAsync(2);
            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetUserById_ShouldNotWork_NotExistingUser()
        {
            User user = await _userService.GetUserByIdAsync(999);
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldWork_ExistingUser()
        {
            User user = await _userService.GetUserByEmailAsync("spider.man@gmail.com");
            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetUserByEmail_ShouldWork_NotExistingUser()
        {
            User user = await _userService.GetUserByEmailAsync("spider.man2@gmail.com");
            Assert.Null(user);
        }

        [Fact]
        public async Task GetUserByFirstName_ShouldWork_ExistingUsers()
        {
            List<User> users = await _userService.GetUserByFirstNameAsync("Jhon");
            Assert.Equal(2, users.Count);

        }

        [Fact]
        public async Task GetUserByFirstName_ShouldWork_NoUsersFound()
        {
            List<User> users = await _userService.GetUserByFirstNameAsync("Michael");
            Assert.Empty(users);
        }

        [Fact]
        public async Task GetUserByLastName_ShouldWork_ExistingUsers()
        { 
            List<User> users = await _userService.GetUserByLastNameAsync("Man");
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task GetUserByLastName_ShouldNotWork_NoUserFound()
        { 
            List<User> users = await _userService.GetUserByLastNameAsync("Jordan");
            Assert.Empty(users);
        }

        [Fact]
        public async Task GetUserByRole_ShouldWork_ExistingUsers()
        {
            List<User> users = await _userService.GetUserByRoleAsync("Customer");
            Assert.Equal(2, users.Count);
        }

        [Fact]
        public async Task GetUserByRole_ShouldNotWork_NoUserFound()
        {
            List<User> users = await _userService.GetUserByRoleAsync("Admin");
            Assert.Empty(users);
        }
    }
}
