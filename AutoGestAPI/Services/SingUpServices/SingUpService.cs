using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;
using AutoGestAPI.Services.AuthServices;
using Microsoft.EntityFrameworkCore;

namespace AutoGestAPI.Services.SingUpServices
{
    public class SingUpService : ISingUpService
    {
        protected readonly AppDb _context;
        protected readonly IAuthService _authService;
        public SingUpService(AppDb context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<string?> SingUp(SingUpDTO dto)
        {
            User? user = await _context.User.Where(u => u.Username == dto.Username ||
                                                u.Email == dto.Username ||
                                                u.Username == dto.Email ||
                                                u.Email == dto.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                User newUser = new User(dto.Username, dto.Email, dto.Password);
                await _context.User.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return await _authService.Login(new UserLoginDTO(dto.Username, dto.Password));
            }
            return null;
        }
    }
}
