using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;

namespace AutoGestAPI.Services.AuthServices
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashPassword);
        Task<string?> Login(UserLoginDTO dto);
        Task<Guid> getUserId();
    }
}
