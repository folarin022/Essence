using EssenceShop.Data;
using EssenceShop.Dto;
using EssenceShop.Dto.ClientsModel;
using EssenceShop.Dto.ClothesModel;

namespace EssenceShop.Service.Interface
{
    public interface IClientsService
    {
        Task<BaseResponse<bool>> AddClients(CreateClientDto request, CancellationToken cancellationToken);
        Task<BaseResponse<List<Data.Clients>>> GetAllClients(CancellationToken cancellationToken);
        Task<BaseResponse<Clients>> GetClientsById(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteClients(Guid Id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateClients(Guid Id, UpdateClientsDto request, CancellationToken cancellationToken);

    }
}
