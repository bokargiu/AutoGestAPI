using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;

namespace AutoGestAPI.Services.ClientServices
{
    public interface IClientService
    {
        Task<List<Client>> getClientForUser();
        Task postClient(ClientDto dto);
        Task dellByUserId(string idString);
    }
}
