using EssenceShop.Context;
using EssenceShop.Dto.AuthModel;
using EssenceShop.Repositries.Interface;

namespace EssenceShop.Repositries
{
    public class AuthRepositries(EssenceDbContext dbContext) : IAuthRepositries
    {

        public async Task<bool> Login(LoginUserDto request, CancellationToken cancellationToken)
        {
           await dbContext.AddAsync(request, cancellationToken);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Register(RegisterUserDto request, CancellationToken cancellationToken)
        {
            await dbContext.AddAsync(request,cancellationToken);
            return await dbContext.SaveChangesAsync() > 0;
        }
    }
}
