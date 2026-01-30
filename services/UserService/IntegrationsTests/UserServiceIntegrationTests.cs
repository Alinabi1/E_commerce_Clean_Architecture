using System.Data;
using System.Threading.Tasks;
using UserService.Application.Interfaces;
using UserService.Application.Services;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data;

namespace IntegrationsTests
{
    public class UserServiceIntegrationTests : IntegrationTestBase
    {
        [Fact]
        public async Task AddUser_ShouldPersistInDatabase()
        {
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            string email = "jhon.wick@gmail.com";
            User user = new User("Jhon", "Wick", email, "jhonwick123", "Admin");

            await _service.AddUserAsync(user);

            var createdUser = await _service.GetUserByEmailAsync(email);
            Assert.NotNull(createdUser);

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

            User user = new User(firstName, lastName, email, pass, role);

            await _service.AddUserAsync(user);

            var CreatedUser = await _service.GetUserByEmailAsync("jhon.wick@gmail.com");
            Assert.NotNull(CreatedUser);
            Assert.Equal(firstName, CreatedUser.FirstName);
            Assert.Equal(lastName, CreatedUser.LastName);
            Assert.Equal(email, CreatedUser.Email);
            Assert.Equal(role, CreatedUser.Role.ToString());

            CleanupDatabase(_context);
        }

        [Fact]
        public async Task UppdateUser_ShouldSaveChangesInDatabase()
        { 
            var _context = CreateDbContext();
            var _repo = new UserRepository(_context);
            var _service = new UserServiceImp(_repo);

            string email = "jhon.wick@gmail.com";
            User user = new User("Jhon", "Wick", email, "jhonwick123", "Admin");

            await _service.AddUserAsync(user);

            string newFirstName = "Jonathan";
            string newLastName = "Wickerson";
            string newEmail = "jonathan.wickerson@gmail.com";
            string newPassword = "newpassword123";

            User userForUpdate = new User(newFirstName, newLastName, newEmail, newPassword);

            var createdUser = await _service.GetUserByEmailAsync(email);
            Assert.NotNull(createdUser);

            await _service.UpdateUserAsync(createdUser.Id, userForUpdate);

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

            User user = new User("Jhon", "Wick", email, "jhonwick123", "Admin");

            await _service.AddUserAsync(user);

            var CreatedUser = await _service.GetUserByEmailAsync(email);
            Assert.NotNull(CreatedUser);

            await _service.RemoveUserAsync(CreatedUser.Id);

            User? deletedUser = await _service.GetUserByIdAsync(CreatedUser.Id);
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
            User user1 = new User(firstName, "Wick", "jhon.wick@gamil.com", "jhonwick123", "Admin");
            User user2 = new User(firstName, "Wickerson", "jhon.wickerson@gamil.com", "jhonwickerson123", "Admin");
            User user3 = new User("Bat", "Man", "bat.man@gmail.com", "batman123", "Owner");


            await _service.AddUserAsync(user1);
            await _service.AddUserAsync(user2);
            await _service.AddUserAsync(user3);

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

            User user1 = new User("Jhon", lastName, "jhon.wick@gamil.com", "jhonwick123", "Admin");
            User user2 = new User("Jonathan", lastName, "jonathan.wick@gamil.com", "jonathanwick123", "Admin");
            User user3 = new User("Bat", "Man", "bat.man@gmail.com", "batman123", "Owner");

            await _service.AddUserAsync(user1);
            await _service.AddUserAsync(user2);
            await _service.AddUserAsync(user3);

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

            User user1 = new User("Jhon", "Wick", "jhon.wick@gamil.com", "jhonwick123", role);
            User user2 = new User("Jonathan", "Wickerson", "jonathan.wickerson@gamil.com", "jhonwickerson123", role);
            User user3 = new User("Bat", "Man", "bat.man@gmail.com", "batman123", "Owner");

            await _service.AddUserAsync(user1);
            await _service.AddUserAsync(user2);
            await _service.AddUserAsync(user3);

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

            User user1 = new User("Jhon", "Wick", "jhon.wick@gamil.com", "jhonwick123", "Admin");
            User user2 = new User("Jonathan", "Wickerson", "jonathan.wickerson@gamil.com", "jhonwickerson123", "Admin");
            User user3 = new User("Bat", "Man", "bat.man@gmail.com", "batman123", "Owner");

            await _service.AddUserAsync(user1);
            await _service.AddUserAsync(user2);
            await _service.AddUserAsync(user3);

            var users = await _service.GetAllUsersAsync();
            Assert.NotNull(users);
            Assert.Equal(3, users.Count);

            CleanupDatabase(_context);

        }

    }
}
