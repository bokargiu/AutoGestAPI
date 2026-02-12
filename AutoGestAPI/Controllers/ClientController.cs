using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;
using AutoGestAPI.Services;
using AutoGestAPI.Services.AuthServices;
using AutoGestAPI.Services.ClientServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoGestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        protected readonly IClientService _client;
        public ClientController(IClientService client)
        {
            _client = client;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> getClients()
        {
            var clients = await _client.getClientsByUserId();
            return Ok(new { clients });
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> postClient([FromBody] ClientDto dto)
        {
            if (dto.Number.Length != 11) return BadRequest();
            await _client.postClient(dto);
            return Ok();
        }
        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> dellById([FromRoute] string Id)
        {
            await _client.dellByUserId(Id);
            return Ok();
        }
    }
}
