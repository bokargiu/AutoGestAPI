namespace AutoGestAPI.Models
{
    public class Service
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public double Price { get; set; }
        public int DurationMin { get; set; }
        public ICollection<OrderAndService> OrderAndServices { get; set; } = new List<OrderAndService>();

        public Guid UserId { get; set; }
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
