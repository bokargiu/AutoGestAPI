using BCrypt.Net;
using System.Text.Json.Serialization;

namespace AutoGestAPI.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<Client> Clients { get; set; } = new List<Client>();
        public ICollection<Service> Services { get; set; } = new List<Service>();

        public User() { }

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
