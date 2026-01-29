using AutoGestAPI.DTO_s;
using AutoGestAPI.Services.ServiceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoGestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _service;
        public ServiceController(IServiceService service)
        {
            _service = service;
        }
        [HttpGet, Authorize]
        public async Task<IActionResult> getServices()
        {
            var services = await _service.getServicesByUserId();
            return Ok(new { services });
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> postService([FromBody] ServiceDto dto)
        {
            await _service.postService(dto);
            return Ok();
        }
        [HttpDelete("{Id}"), Authorize]
        public async Task<IActionResult> dellService([FromRoute] string Id)
        {
            await _service.dellService(Id);
            return Ok();
        }
    }
}
