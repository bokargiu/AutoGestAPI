using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Exceptions;
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
        public async Task<List<Client>> getClientsByUserId()
        {
            Guid? userId = await _auth.getUserId();
            return await _context.Client.Where(c => userId == c.UserId).ToListAsync();
        }
        public async Task<Client> getClientById(string idString)
        {
            if (!Guid.TryParse(idString, out Guid id)) throw new BadRequestException("Client Id inválido");

            Client? client = await _context.Client.Where(c =>  Guid.Parse(idString) == c.Id).FirstOrDefaultAsync();

            if (client == null) throw new NotFoundException("Client não encontrado");
            if (await _auth.getUserId() != client.UserId) throw new UnauthorizedException("Não Autorizado");

            return client;
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
            Client client = await getClientById(idString);

            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return;
        }
    }
}
