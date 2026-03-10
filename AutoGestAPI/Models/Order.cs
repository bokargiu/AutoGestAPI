using System.Text.Json.Serialization;

namespace AutoGestAPI.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double TotalPrice { get; set; }
        [JsonIgnore]
        public Client Client { get; set; }
        [JsonIgnore]
        public Guid ClientId { get; set; }
        [JsonIgnore]
        public ICollection<OrderAndService> OrdersAndServices { get; set; } = new List<OrderAndService>();

        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        public Order() { }
        public Order(DateTime start)
        {
            this.Start = start;
        }
    }
}
