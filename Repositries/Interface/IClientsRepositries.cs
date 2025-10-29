using EssenceShop.Data;
using EssenceShop.Dto.ClientsModel;

namespace EssenceShop.Repositries.Interface
{
    public interface IClientsRepositries
    {
        Task<bool> AddClient(CreateClientDto request,CancellationToken cancellationToken);
        Task<Clients?> GetClientsById(Guid Id,CancellationToken cancellationToken);
        Task<List<Clients>> GetAllClients(CancellationToken cancellationToken);
        Task<bool> UpdateClientsDto(Guid Id,UpdateClientsDto request,CancellationToken cancellationToken);
        Task<bool> DeleteClients(Guid Id,CancellationToken cancellationToken);
        Task AddAsync(CreateClientDto request);
        Task SaveChangesAsync();
    }
}
