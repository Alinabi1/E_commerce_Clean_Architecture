using System.Threading.Tasks;
using UserService.Infrastructure.Data;
using UserService.Application.Services;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace IntegrationsTests
{
    public class UserServiceIntgrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task AddUser_ShouldPersistInDatabase()
        {
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            string email = "jhon.wick@gmail.com";

            await _service.AddUserAsync("Jhon", "Wick", email, "jhonwick123", "Admin");

            var user = await _service.GetUserByEmailAsync(email);
            Assert.NotNull(user);

            CleanupDatabase(_context);

        }

        [Fact]
        public async Task GetUserByEmail_ShouldReturnCorrectUser()
        {
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            string firstName = "Jhon";
            string lastName = "Wick";
            string email = "jhon.wick@gmail.com";
            string pass = "jhonwick123";
            string role = "Admin";

            await _service.AddUserAsync(firstName,lastName,email,pass,role);

            var user = await _service.GetUserByEmailAsync("jhon.wick@gmail.com");
            Assert.NotNull(user);
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
            Assert.Equal(email, user.Email);
            Assert.Equal(role, user.Role.ToString());

            CleanupDatabase(_context);
        }

        [Fact]
        public async Task UppdateUser_ShouldSaveChangesInDatabase()
        { 
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            string email = "jhon.wick@gmail.com";

            await _service.AddUserAsync("Jhon", "Wick", email, "jhonwick123", "Admin");

            string newFirstName = "Jonathan";
            string newLastName = "Wickerson";
            string newEmail = "jonathan.wickerson@gmail.com";
            string newPassword = "newpassword123";

            var user = await _service.GetUserByEmailAsync(email);
            Assert.NotNull(user);

            await _service.ChangeFirstNameAsync(user.Id, newFirstName);
            await _service.ChangeLastNameAsync(user.Id, newLastName);
            await _service.ChangePasswordAsync(user.Id, newPassword);
            await _service.ChangeEmailAsync(user.Id, newEmail);

            var updatedUser = await _service.GetUserByEmailAsync(newEmail);

            Assert.NotNull(updatedUser);
            Assert.Equal(newFirstName, updatedUser.FirstName);
            Assert.Equal(newLastName, updatedUser.LastName);
            Assert.Equal(newEmail, updatedUser.Email);
            Assert.Equal(newPassword, updatedUser.PasswordHash);

            CleanupDatabase(_context);

        }

        [Fact]
        public async Task DeleteUser_ShouldRemoveFromDatabase()
        {
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            string email = "jhon.wick@gamil.com";

            await _service.AddUserAsync("Jhon", "Wick", email, "jhonwick123", "Admin");

            var user = await _service.GetUserByEmailAsync(email);
            Assert.NotNull(user);

            await _service.RemoveUserAsync(user.Id);

            User? deletedUser = await _service.GetUserByIdAsync(user.Id);
            Assert.Null(deletedUser);

            CleanupDatabase(_context);
        }

        [Fact]
        public async Task GetUserByFirstName_ShouldReturnCorrect()
        {
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            string firstName = "Jhon";

            await _service.AddUserAsync(firstName, "Wick", "jhon.wick@gamil.com", "jhonwick123", "Admin");
            await _service.AddUserAsync(firstName, "Wickerson", "jhon.wickerson@gamil.com", "jhonwickerson123", "Admin");
            await _service.AddUserAsync("Bat", "Man", "bat.man@gmail.com", "batman123", "Owner");

            var list = await _service.GetUserByFirstNameAsync(firstName);
            Assert.NotNull(list);
            Assert.Equal(2,list.Count);

            CleanupDatabase(_context);
        }

        [Fact]
        public async Task GetUserByLastName_ShouldReturnCorrect()
        {
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            string lastName = "Wick";

            await _service.AddUserAsync("Jhon", lastName, "jhon.wick@gamil.com", "jhonwick123", "Admin");
            await _service.AddUserAsync("Jonathan", lastName, "jonathan.wick@gamil.com", "jonathanwick123", "Admin");
            await _service.AddUserAsync("Bat", "Man", "bat.man@gmail.com", "batman123", "Owner");

            var list = await _service.GetUserByLastNameAsync(lastName);
            Assert.NotNull(list);
            Assert.Equal(2, list.Count);

            CleanupDatabase(_context);

        }

        [Fact]
        public async Task GetUserByRole_ShouldReturnCorrect()
        {
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            string role = "Admin";

            await _service.AddUserAsync("Jhon", "Wick", "jhon.wick@gamil.com", "jhonwick123", role);
            await _service.AddUserAsync("Jonathan", "Wickerson", "jonathan.wickerson@gamil.com", "jhonwickerson123", role);
            await _service.AddUserAsync("Bat", "Man", "bat.man@gmail.com", "batman123", "Owner");

            var users = await _service.GetUserByRoleAsync(role);
            Assert.NotNull(users);
            Assert.Equal(2, users.Count);

            CleanupDatabase(_context);

        }

        [Fact]
        public async Task GetUserAll_ShouldReturnCorrect()
        {
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            await _service.AddUserAsync("Jhon", "Wick", "jhon.wick@gamil.com", "jhonwick123", "Admin");
            await _service.AddUserAsync("Jonathan", "Wickerson", "jonathan.wickerson@gamil.com", "jhonwickerson123", "Admin");
            await _service.AddUserAsync("Bat", "Man", "bat.man@gmail.com", "batman123", "Owner");

            var users = await _service.GetAllUsersAsync();
            Assert.NotNull(users);
            Assert.Equal(3, users.Count);

            CleanupDatabase(_context);

        }

    }
}
