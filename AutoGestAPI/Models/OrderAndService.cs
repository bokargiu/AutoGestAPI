using System.Text.Json.Serialization;

namespace AutoGestAPI.Models
{
    public class OrderAndService
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
