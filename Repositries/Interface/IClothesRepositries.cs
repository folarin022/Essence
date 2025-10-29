using EssenceShop.Data;
using EssenceShop.Dto.ClothesModel;

namespace EssenceShop.Repositries.Interface
{
    public interface IClothesRepositries
    {
        Task<bool> AddClothes(Clothes clothes, CancellationToken cancellationToken);
        Task<List<Clothes>> GetAllClothes(CancellationToken cancellationToken);
        Task<Clothes>GetClothesById(Guid Id,  CancellationToken cancellationToken);
        Task<bool> UpdateClothe(Clothes clothes, CancellationToken cancelllationToken);
        Task<bool> DeleteClothes(Guid Id,CancellationToken cancellationToken);
        Task SaveChangesAsync();
        Task AddAsync(CreateClothesDto request);
    }
}
