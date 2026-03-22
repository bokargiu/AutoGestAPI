using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Exceptions;
using AutoGestAPI.Models;
using AutoGestAPI.Services.AuthServices;
using AutoGestAPI.Services.ClientServices;
using AutoGestAPI.Services.ServiceServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

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
            Order? order = await _context.Order.Where(o => Guid.Parse(idString) == o.Id)
                                                .Include(o => o.Client)
                                                .Include(o => o.OrdersAndServices)
                                                    .ThenInclude(os => os.Service)
                                                .FirstOrDefaultAsync();
            if (order == null) throw new NotFoundException("Order não encontrada");
            if (order.UserId != await _auth.getUserId()) throw new UnauthorizedException("Não Autorizado");
            return order;
        }
        public async Task<OrderResponseDto> getOrderDtoById(string idString)
        {
            if (!Guid.TryParse(idString, out Guid id)) throw new BadRequestException("Order Id Inválido");
            Order? order = await _context.Order.Where(o => Guid.Parse(idString) == o.Id)
                                                .Include(o => o.Client)
                                                .Include(o => o.OrdersAndServices)
                                                    .ThenInclude(os => os.Service)
                                                .FirstOrDefaultAsync();
            if (order == null) throw new NotFoundException("Order não encontrada");
            if (order.UserId != await _auth.getUserId()) throw new UnauthorizedException("Não Autorizado");
            return new OrderResponseDto
            {
                Id = order.Id,
                Start = order.Start,
                End = order.End,
                TotalPrice = order.TotalPrice,
                Client = order.Client,
                Services = order.OrdersAndServices.Select(os => os.Service).ToList()
            };
        }
        public async Task<List<OrderResponseDto>> getOrdersByUserId()
        {
            Guid id = await _auth.getUserId();
            var orders = await _context.Order.Where(o => id == o.UserId)
                                                .Include(o => o.Client)
                                                .Include(o => o.OrdersAndServices)
                                                    .ThenInclude(os => os.Service)
                                                .ToListAsync();
            List<OrderResponseDto> response = orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                Start = o.Start,
                End = o.End,
                TotalPrice = o.TotalPrice,
                Client = o.Client,
                Services = o.OrdersAndServices.Select(os => os.Service).ToList()
            }).ToList();
            return response;
        }
        public async Task<List<OrderResponseDto>> getOrdersByMonthAndUserId(DateTime date)
        {
            List<OrderResponseDto> list = await getOrdersByUserId();
            list = list.Where(o => date.Month == o.Start.Month 
                                && date.Year == o.Start.Year)
                        .ToList();
            return list;
        }
        public async Task postOrder(OrderDto dto)
        {
            Guid userId = await _auth.getUserId();

            if (!Guid.TryParse(dto.ClientId, out Guid clientId))
                throw new BadRequestException("ClientId inválido");

            Client client = await _client.getClientById(dto.ClientId);

            Order order = new Order
            {
                Start = dto.Start,
                End = dto.Start,
                UserId = userId,
                ClientId = clientId
            };

            await _context.Order.AddAsync(order);

            List<Guid> serviceIds = dto.ServicesIds
                .Select(id =>
                {
                    if (!Guid.TryParse(id, out Guid guid))
                        throw new BadRequestException("ServiceId inválido");
                    return guid;
                })
                .ToList();

            List<Service> services = await _context.Service
                .Where(s => serviceIds.Contains(s.Id))
                .ToListAsync();

            foreach (Service service in services)
            {
                if (service.UserId != userId)
                    throw new UnauthorizedException("Service não pertence ao usuário");

                OrderAndService os = new OrderAndService
                {
                    OrderId = order.Id,
                    ServiceId = service.Id
                };

                order.TotalPrice += service.Price;
                order.End = order.End.AddMinutes(service.DurationMin);

                order.OrdersAndServices.Add(os);
            }

            await _context.SaveChangesAsync();
        }
        public async Task patchOrder(OrderDto dto, string idString)
        {
            Order order = await getOrderById(idString);
            order.Start = dto.Start;
            if(!Guid.TryParse(dto.ClientId, out Guid cId))
                throw new BadRequestException("ClientId não é valido");
            order.ClientId = cId;
            List<Service> services = await _service.getServicesByUserId();

            List<Guid> dto_sId = dto.ServicesIds.Select(ids => {
                if (!Guid.TryParse(ids, out Guid guid))
                    throw new BadRequestException("ServiceId não é valido");
                return guid;
            }).ToList();

            List<Guid> os_sId = order.OrdersAndServices
                                    .Select(os => os.ServiceId)
                                    .ToList();

            foreach (Guid sId in dto_sId.Except(os_sId))
            {
                if (!services.Exists(s => sId == s.Id))
                    throw new BadRequestException("ServiceId não pertence ao usuário");
                order.OrdersAndServices.Add(new OrderAndService
                {
                    OrderId = order.Id,
                    ServiceId = sId
                });
            }

            List<OrderAndService> servicesToRemove = order.OrdersAndServices
                                            .Where(os => !dto_sId.Contains(os.ServiceId))
                                            .ToList();
            servicesToRemove.ForEach(sDell => order.OrdersAndServices.Remove(sDell));

            order.TotalPrice = order.OrdersAndServices.Sum(os => services.Where(s => s.Id == os.ServiceId).First().Price);
            order.End = order.Start.AddMinutes(order.OrdersAndServices.Sum(os => services.Where(s => s.Id == os.ServiceId).First().DurationMin));
            await _context.SaveChangesAsync();
        }
        public async Task dellOrderById(string idString)
        {

            Order order = await getOrderById(idString);

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}
