using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;
using AutoGestAPI.Services.AuthServices;
using Microsoft.EntityFrameworkCore;

namespace AutoGestAPI.Services.SingUpServices
{
    public class UserService : IUserService
    {
        protected readonly AppDb _context;
        protected readonly IAuthService _authService;
        private readonly HttpClient _http;
        public UserService(AppDb context, IAuthService authService, IHttpClientFactory httpClientBuilder)
        {
            _context = context;
            _authService = authService;
            _http = httpClientBuilder.CreateClient();
        }
        public async Task<string?> SingUp(SingUpDTO dto)
        {
            var response = await _http.PostAsJsonAsync("https://bokargiu.com.br/api/User", dto);
            return (int)response.StatusCode == StatusCodes.Status201Created ? await response.Content.ReadAsStringAsync() : null;

        }
    }
}
