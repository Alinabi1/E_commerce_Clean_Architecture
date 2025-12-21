using UserService.Domain.Entities;
namespace TestUser
{
    public class TestUser
    {
        [Fact]
        public void Create_ShouldWork_AllDataValid()
        {
            string firstName= "john";
            string lastName = "Wick";
            string email = "john.wick@gamil.com";
            string passwordHash = "Hash";
            string role = "Customer";
            User user = new (firstName,lastName,email,passwordHash,role);

            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
            Assert.Equal(email, user.Email);
            Assert.Equal(passwordHash, user.PasswordHash);
            Assert.Equal(UserRole.Customer, user.Role);
        }

        [Fact]
        public void Create_ShouldFail_InvalidEmail()
        {
            string firstName = "john";
            string lastName = "Wick";
            string email = "john.wick-gamil.com";
            string passwordHash = "Hash";
            string role = "Customer";
            var exception = Assert.Throws<ArgumentException>(() => new User(firstName, lastName, email, passwordHash, role));
            Assert.Equal("Invalid email!", exception.Message);
        }
    }
}
