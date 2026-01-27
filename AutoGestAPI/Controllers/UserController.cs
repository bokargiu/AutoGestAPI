using AutoGestAPI.DTO_s;
using AutoGestAPI.Services.AuthServices;
using AutoGestAPI.Services.SingUpServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoGestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly IAuthService _auth;
        protected readonly ISingUpService _singUp;
        public UserController(IAuthService auth, ISingUpService singUp)
        {
            _auth = auth;
            _singUp = singUp;
        }

        [HttpPost("SingUp")]
        public async Task<IActionResult> SingUp([FromBody] SingUpDTO dto)
        {
            string? result = await _singUp.SingUp(dto);
            if(result != null)
            {
                return Ok(new { result });
            }
            return BadRequest();
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            string? result = await _auth.Login(dto);
            if (result != null)
            {
                return Ok(new { result });
            }
            return BadRequest();
        }
    }
}
