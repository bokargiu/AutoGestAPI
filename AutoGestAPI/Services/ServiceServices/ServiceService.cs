using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Models;
using AutoGestAPI.Services.AuthServices;
using Microsoft.EntityFrameworkCore;

namespace AutoGestAPI.Services.ServiceServices
{
    public class ServiceService : IServiceService
    {
        private readonly IAuthService _auth;
        private readonly AppDb _context;
        public ServiceService(IAuthService auth, AppDb context)
        {
            _auth = auth;
            _context = context;
        }
        public async Task<List<Service>> getServicesByUserId()
        {
            Guid userId = await _auth.getUserId();
            return await _context.Service.Where(s => userId == s.UserId).ToListAsync();
        }
        public async Task postService(ServiceDto dto)
        {
            Service service = new Service();
            service.Title = dto.Title;
            service.Price = dto.Price;
            service.DurationMin = dto.DurationMin;
            service.UserId = await _auth.getUserId();

            await _context.Service.AddAsync(service);
            await _context.SaveChangesAsync();
        }
        public async Task dellService(string serviceId)
        {
            Guid id = Guid.Parse(serviceId);
            Service? service = await _context.Service.Where(s => id == s.Id).FirstOrDefaultAsync();
            if (service == null) throw new Exception("Service não encontrado");
            if (service.UserId != await _auth.getUserId()) throw new Exception("Não Autorizado");

            _context.Service.Remove(service);
            await _context.SaveChangesAsync();
            return;
        }
    }
}
