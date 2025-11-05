using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto.AuthModel;
using EssenceShop.Repositries.Interface;

namespace EssenceShop.Repositries
{
    public class AdminRepositries(EssenceDbContext dbContext) : IAdminRepository
    {
        internal static async Task AddAsync(Admin admin)
        {
            throw new NotImplementedException();
        }

        internal static async Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Login(AdminLoginDto request, CancellationToken cancellationToken)
        {
            await dbContext.AddAsync(request, cancellationToken);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegisterAdmin(AdminLoginDto request, CancellationToken cancellationToken)
        {
            await dbContext.AddAsync(request, cancellationToken);
            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
