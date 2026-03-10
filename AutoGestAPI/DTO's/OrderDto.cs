using AutoGestAPI.Models;

namespace AutoGestAPI.DTO_s
{
    public class OrderDto
    {
        public DateTime Start { get; set; }
        public string ClientId { get; set; }
        public List<string> ServicesIds { get; set; }

    }
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public double TotalPrice { get; set; }
        public Client Client { get; set; }
        public List<Service> Services { get; set; }
    }

}
