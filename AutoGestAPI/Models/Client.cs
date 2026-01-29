namespace AutoGestAPI.Models
{
    public class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Number { get; set; } = string.Empty;

        public Guid UserId { get; set; } = Guid.Empty;
        public User User { get; set; }

        public Client() { }
        public Client(string name, string number)
        {
            this.Name = name;
            this.Number = number;
        }
    }
}
