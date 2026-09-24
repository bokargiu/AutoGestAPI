using AutoGestAPI.DTO_s;

namespace AutoGestAPI.Services.SingUpServices
{
    public interface IUserService
    {
        Task<string> SingUp(SingUpDTO dto);
    }
}
