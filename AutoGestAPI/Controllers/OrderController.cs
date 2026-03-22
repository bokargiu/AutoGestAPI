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
        [HttpGet("{id}"), Authorize]
        public async Task<IActionResult> getOrder([FromRoute]string id)
        {
            OrderResponseDto order = await _order.getOrderDtoById(id);
            return Ok( order );
        }
        [HttpGet("Date:{date}"), Authorize]
        public async Task<IActionResult> getOrdersOfMonth([FromRoute]DateTime date)
        {
            return Ok(await _order.getOrdersByMonthAndUserId(date));
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> postOrder([FromBody] OrderDto dto)
        {
            await _order.postOrder(dto);
            return Ok();
        }
        [HttpPatch("{id}"), Authorize]
        public async Task<IActionResult> patchOrder([FromBody] OrderDto dto, [FromRoute] string id)
        {
            await _order.patchOrder(dto, id);
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
