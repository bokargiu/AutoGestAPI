using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;

namespace AutoGestAPI.Services.AuthServices
{
    public interface IAuthService
    {
        string GenerateJwt(User user);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashPassword);
        Task<string?> Login(UserLoginDTO dto);
    }
}
