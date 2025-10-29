using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto.ClothesModel;
using EssenceShop.Repositries.Interface;
using Microsoft.EntityFrameworkCore;

namespace EssenceShop.Repositries
{
    public class ClothesRepositries : IClothesRepositries
    {
        private readonly EssenceDbContext _dbContext;

        public ClothesRepositries(EssenceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddClothes(Clothes clothes, CancellationToken cancellationToken)
        {
            await _dbContext.Clothes.AddAsync(clothes, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteClothes(Guid id, CancellationToken cancellationToken)
        {
            var clothe = await _dbContext.Clothes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            if (clothe == null) return false;

            _dbContext.Clothes.Remove(clothe);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<List<Clothes>> GetAllClothes(CancellationToken cancellationToken)
        {
            return await _dbContext.Clothes.ToListAsync(cancellationToken);
        }

        public async Task<Clothes?> GetClothesById(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Clothes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<bool> UpdateClothe(Clothes clothes, CancellationToken cancellationToken)
        {
            _dbContext.Clothes.Update(clothes);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public Task AddAsync(CreateClothesDto request)
        {
            throw new NotImplementedException();
        }
    }
}
