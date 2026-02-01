using AutoGestAPI.Models;

namespace AutoGestAPI.DTO_s
{
    public class OrderDto
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public string ClientId { get; set; }
        public List<string> ServicesIds { get; set; }

    }
}
