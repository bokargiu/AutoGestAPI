using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Exceptions;
using AutoGestAPI.Models;
using AutoGestAPI.Services.AuthServices;
using AutoGestAPI.Services.ClientServices;
using AutoGestAPI.Services.ServiceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoGestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDb _context;
        private readonly IAuthService _auth;
        private readonly IClientService _client;
        private readonly IServiceService _service;
        public OrderController(AppDb context, IAuthService auth, IClientService client, IServiceService service)
        {
            _context = context;
            _auth = auth;
            _client = client;
            _service = service;
        }
        [HttpGet, Authorize]
        public async Task<IActionResult> getOrders()
        {
            Guid id = await _auth.getUserId();
            return Ok( await _context.Order.Where(o => id == o.UserId).ToListAsync() );
        }
        [HttpGet("{idString}"), Authorize]
        public async Task<IActionResult> getOrder([FromRoute]string idString)
        {
            if (!Guid.TryParse(idString, out Guid id)) throw new BadRequestException("Order Id Inválido");
            Order? order = await _context.Order.Where(o => Guid.Parse(idString) == o.Id).FirstOrDefaultAsync();
            if (order == null) throw new NotFoundException("Order não encontrada");
            if (order.UserId != await _auth.getUserId()) throw new UnauthorizedException("Não Autorizado");
            return Ok( new { order } );
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> postOrder([FromBody] OrderDto dto)
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
            };
            await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> dellOrder(string id)
        {
            Order order = (Order)await getOrder(id);

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
