namespace AutoGestAPI.Models
{
    public class OrderAndService
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
