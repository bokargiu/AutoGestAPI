using AutoGestAPI.Database;
using AutoGestAPI.DTO_s;
using AutoGestAPI.Exceptions;
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
        public async Task<Service> getServiceById(string idString)
        {
            if (!Guid.TryParse(idString, out Guid id)) throw new BadRequestException("Service Id inválido");

            Service? service = await _context.Service.Where(s => Guid.Parse(idString) == s.Id).FirstOrDefaultAsync();

            if (service == null) throw new NotFoundException("Service não encontrado");
            if (service.UserId != await _auth.getUserId()) throw new UnauthorizedException("Não autorizado");
            return service;

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
            Service service = await getServiceById(serviceId);

            _context.Service.Remove(service);
            await _context.SaveChangesAsync();
            return;
        }
    }
}
