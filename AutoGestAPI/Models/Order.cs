namespace AutoGestAPI.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateOnly Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public double TotalPrice { get; set; }
        public Client Client { get; set; }
        public ICollection<OrderAndService> OrdersAndServices { get; set; } = new List<OrderAndService>();
        public Guid UserId { get; set; }
        public User User { get; set; }

        public Order() { }
        public Order(DateOnly day, TimeOnly start)
        {
            this.Day = day;
            this.StartTime = start;
        }
    }
}
