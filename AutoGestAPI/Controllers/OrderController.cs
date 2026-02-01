using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Exceptions;
using AutoGestAPI.Models;
using AutoGestAPI.Services.AuthServices;
using AutoGestAPI.Services.ClientServices;
using AutoGestAPI.Services.OrderServices;
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
        private readonly IOrderService _order;
        public OrderController(IOrderService order)
        {
            _order = order;
        }
        [HttpGet, Authorize]
        public async Task<IActionResult> getOrders()
        {
            return Ok( await _order.getOrdersByUserId() );
        }
        [HttpGet("{idString}"), Authorize]
        public async Task<IActionResult> getOrder([FromRoute]string id)
        {
            Order order = await _order.getOrderById(id);
            return Ok( new { order } );
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> postOrder([FromBody] OrderDto dto)
        {
            await _order.postOrder(dto);
            return Ok();
        }
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> dellOrder(string id)
        {
            await _order.dellOrderById(id);
            return Ok();
        }
    }
}
