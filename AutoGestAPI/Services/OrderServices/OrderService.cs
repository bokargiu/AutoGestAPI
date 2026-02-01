using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Exceptions;
using AutoGestAPI.Models;
using AutoGestAPI.Services.AuthServices;
using AutoGestAPI.Services.ClientServices;
using AutoGestAPI.Services.ServiceServices;
using Microsoft.EntityFrameworkCore;

namespace AutoGestAPI.Services.OrderServices
{
    public class OrderService : IOrderService
    {
        private readonly AppDb _context;
        private readonly IAuthService _auth;
        private readonly IClientService _client;
        private readonly IServiceService _service;
        public OrderService(AppDb context, IAuthService auth, IClientService client, IServiceService service)
        {
            _context = context;
            _auth = auth;
            _client = client;
            _service = service;
        }

        public async Task<Order> getOrderById(string idString)
        {
            if (!Guid.TryParse(idString, out Guid id)) throw new BadRequestException("Order Id Inválido");
            Order? order = await _context.Order.Where(o => Guid.Parse(idString) == o.Id).FirstOrDefaultAsync();
            if (order == null) throw new NotFoundException("Order não encontrada");
            if (order.UserId != await _auth.getUserId()) throw new UnauthorizedException("Não Autorizado");
            return order;
        }
        public async Task<List<Order>> getOrdersByUserId()
        {
            Guid id = await _auth.getUserId();
            return await _context.Order.Where(o => id == o.UserId).ToListAsync();
        }
        public async Task postOrder(OrderDto dto)
        {
            Guid userId = await _auth.getUserId();
            Order order = new Order()
            {
                Day = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.StartTime,
                Client = await _client.getClientById(dto.ClientId),
                UserId = userId
            };
            foreach (string sId in dto.ServicesIds)
            {
                Service s = await _service.getServiceById(sId);
                OrderAndService os = new OrderAndService()
                {
                    Order = order,
                    Service = s
                };
                order.TotalPrice += s.Price;
                order.EndTime = order.EndTime.AddMinutes(s.DurationMin);
                order.OrdersAndServices.Add(os);
            }
            ;
            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();
            return;
        }
        public async Task dellOrderById(string idString)
        {

            Order order = await getOrderById(idString);

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return;
        }
    }
}
