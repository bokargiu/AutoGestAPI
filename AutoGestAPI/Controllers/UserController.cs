using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;
using AutoGestAPI.Services;
using AutoGestAPI.Services.AuthServices;
using AutoGestAPI.Services.SingUpServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoGestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly IAuthService _auth;
        protected readonly IUserService _user;
        public UserController(IAuthService auth, IUserService user)
        {
            _auth = auth;
            _user = user;
        }

        [HttpPost("SingUp")]
        public async Task<IActionResult> SingUp([FromBody] SingUpDTO dto)
        {
            string? result = await _user.SingUp(dto);
            if(result != null)
            {
                return Ok(new { result });
            }
            return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            string? result = await _auth.Login(dto);
            if (result != null)
            {
                return Ok(new { result });
            }
            return BadRequest();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserLoginDTO dto)
        {
            //deploy 3
            return Ok();
        }
    }
}
