using System.Text.Json.Serialization;

namespace AutoGestAPI.Models
{
    public class Service
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public int DurationMin { get; set; }
        [JsonIgnore]
        public ICollection<OrderAndService> OrderAndServices { get; set; } = new List<OrderAndService>();

        [JsonIgnore]
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        public Service() { }
        public Service(string title, double price, int durationMin, User user)
        {
            this.Title = title;
            this.Price = price;
            this.DurationMin = durationMin;
            this.UserId = user.Id;
            this.User = user;
        }
    }
}
