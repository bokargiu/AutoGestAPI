using System.Text.Json.Serialization;

namespace AutoGestAPI.Models
{
    public class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;
        public int Rating { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; } = Guid.Empty;

        [JsonIgnore]
        public User User { get; set; }

        public Client() { }
        public Client(string name, string number, int rating)
        {
            this.Name = name;
            this.Number = number;
            this.Rating = rating;
        }
    }
}
