using AutoGestAPI.DTO_s;

namespace AutoGestAPI.Services.SingUpServices
{
    public interface ISingUpService
    {
        Task<string> SingUp(SingUpDTO dto);
    }
}
