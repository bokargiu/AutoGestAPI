using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using AutoGestAPI.Exceptions;

namespace AutoGestAPI.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        protected readonly IConfiguration _configuration;
        protected readonly AppDb _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _http;

        public AuthService(IConfiguration configuration, AppDb context, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _http = httpClientFactory.CreateClient();
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
        public async Task<string?> Login(UserLoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync("https://bokargiu.com.br/api/User/Login", dto);
            return (int)response.StatusCode == StatusCodes.Status200OK ? await response.Content.ReadAsStringAsync() : null;
        }
        public async Task<Guid> getUserId()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            var claim = user?.FindFirst(ClaimTypes.PrimarySid)
                     ?? user?.FindFirst(JwtRegisteredClaimNames.Sub)
                     ?? user?.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new UnauthorizedException("Não Autorizado");

            return Guid.Parse(claim.Value);
        }
    }
}
