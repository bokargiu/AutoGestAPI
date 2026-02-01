using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;

namespace AutoGestAPI.Services.OrderServices
{
    public interface IOrderService
    {
        Task<Order> getOrderById(string idString);
        Task<List<Order>> getOrdersByUserId();
        Task postOrder(OrderDto dto);
        Task dellOrderById(string idString);
    }
}
