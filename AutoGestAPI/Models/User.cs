using BCrypt.Net;

namespace AutoGestAPI.Models
{
    public class User
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public User() { }

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
