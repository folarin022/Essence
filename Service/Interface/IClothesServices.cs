using EssenceShop.Data;
using EssenceShop.Dto;
using EssenceShop.Dto.ClientsModel;
using EssenceShop.Dto.ClothesModel;

namespace EssenceShop.Service.Interface
{
    public interface IClothesServices
    {
        Task<BaseResponse<Clothes>> AddClothes(CreateClothesDto request, CancellationToken cancellationToken);
        Task<BaseResponse<List<Data.Clothes>>> GetAllClothes(CancellationToken cancellationToken);
        Task<BaseResponse<Clothes>> GetClothesById(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> CollectClothe(Guid id, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteClothe(Guid Id , CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateClothes(Guid Id,UpdateClothesDto request, CancellationToken cancellationToken);

    }
}
