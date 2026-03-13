using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;

namespace AutoGestAPI.Services.OrderServices
{
    public interface IOrderService
    {
        Task<Order> getOrderById(string idString);
        Task<OrderResponseDto> getOrderDtoById(string idString);
        Task<List<OrderResponseDto>> getOrdersByUserId();
        Task postOrder(OrderDto dto);
        Task patchOrder(OrderDto dto, string idString);
        Task dellOrderById(string idString);
    }
}
