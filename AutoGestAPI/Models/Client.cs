namespace AutoGestAPI.Models
{
    public class Client
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public char[] Number { get; set; } = new char[11];

        public Guid UserId { get; set; } = Guid.Empty;
        public User User { get; set; }

        public Client() { }
        public Client(string name, char[] number, User user)
        {
            this.Name = name;
            this.Number = number;
            this.UserId = user.Id;
            this.User = user;
        }
    }
}
