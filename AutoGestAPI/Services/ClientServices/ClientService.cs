using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;
using AutoGestAPI.Services.AuthServices;
using Microsoft.EntityFrameworkCore;

namespace AutoGestAPI.Services.ClientServices
{
    public class ClientService : IClientService
    {
        private readonly IAuthService _auth;
        private readonly AppDb _context;
        public ClientService(IAuthService auth, AppDb context)
        {
            _auth = auth;
            _context = context;
        }
        public async Task<List<Client>> getClientForUser()
        {
            Guid? userId = await _auth.getUserId();
            return await _context.Client.Where(c => userId == c.UserId).ToListAsync();
        }
        public async Task postClient(ClientDto dto)
        {
            Client client = new Client();
            client.Name = dto.Name;
            client.Number = dto.Number;

            client.UserId = await _auth.getUserId();
            await _context.Client.AddAsync(client);
            await _context.SaveChangesAsync();
        }
        public async Task dellByUserId(string idString)
        {
            Guid id = Guid.Parse(idString);
            Client? client = await _context.Client.Where(c => id == c.Id).FirstOrDefaultAsync();
            if (client == null)
                throw new Exception("Client não encontrado");
            if (await _auth.getUserId() == client.UserId)
            {
                _context.Client.Remove(client);
                await _context.SaveChangesAsync();
                return;
            }
            throw new Exception("Não Autorizado");
        }
    }
}
