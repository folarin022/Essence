using EssenceShop.Dto.AuthModel;
using EssenceShop.Dto.ClientsModel;

namespace EssenceShop.Repositries.Interface
{
    public interface IAuthRepositries
    {
        Task<bool> Register (RegisterUserDto request,CancellationToken cancellationToken);
        Task<bool> Login (LoginUserDto request,CancellationToken cancellationToken);
    }
}
