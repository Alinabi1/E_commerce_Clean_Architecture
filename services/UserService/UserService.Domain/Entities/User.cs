namespace UserService.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public UserRole Role { get; private set; }

        public User() { }

        public User(string firstName, string lastName, string email, string passwordHash, string role)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetEmail(email);
            SetPasswordHash(passwordHash);
            SetRole(role);
        }

        public User(string firstName, string lastName, string email, string passwordHash)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetEmail(email);
            SetPasswordHash(passwordHash);
        }

        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First-name can not be empty!");
            }
            FirstName = firstName;
        }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last-name can not be empty!");
            }
            LastName = lastName;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            {
                throw new ArgumentException("Invalid email!");
            }
            Email = email;
        }

        public void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                throw new ArgumentException("Password can not be empty!");
            }
            PasswordHash = passwordHash;
        }

        public void SetRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role cannot be empty");
            role = role.Trim();

            if (!Enum.TryParse<UserRole>(role, true, out var parsedRole) ||
               !Enum.IsDefined(typeof(UserRole), parsedRole))
            {
                throw new ArgumentException("Invalid user role!");
            }
            Role = parsedRole;
        }
    }
}
