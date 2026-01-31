using AutoGestAPI.Models;

namespace AutoGestAPI.DTO_s
{
    public class OrderDto
    {
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public Guid ClientId { get; set; }

    }
}
