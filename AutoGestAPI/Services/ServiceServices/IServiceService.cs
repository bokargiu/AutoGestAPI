using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;

namespace AutoGestAPI.Services.ServiceServices
{
    public interface IServiceService
    {
        Task<List<Service>> getServicesByUserId();
        Task postService(ServiceDto dto);
        Task dellService(string serviceId);
    }
}
