using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto.ClientsModel;
using EssenceShop.Repositries.Interface;
using Microsoft.EntityFrameworkCore;

namespace EssenceShop.Repositries
{
    public class ClientsRepositries(EssenceDbContext dbContext) : IClientsRepositries
    {
        public Task AddAsync(CreateClientDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddClient(Clients clients, CancellationToken cancellationToken)
        {
            await dbContext.AddAsync(clients, cancellationToken);
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteClients(Guid Id, CancellationToken cancellationToken)
        {
            dbContext.Remove(Id);
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<List<Clients>> GetAllClients(CancellationToken cancellationToken)
        {
            return await dbContext.Clients.ToListAsync();
        }

        public async Task<Clients?> GetClientsById(Guid Id, CancellationToken cancellationToken)
        {
            return await dbContext.Clients.FirstOrDefaultAsync(c => c.Id == Id, cancellationToken);
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateClientsDto(Guid Id, UpdateClientsDto request, CancellationToken cancellationToken)
        {
            dbContext.Update(Id);
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
