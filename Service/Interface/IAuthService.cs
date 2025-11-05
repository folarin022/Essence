using EssenceShop.Dto;
using EssenceShop.Dto.AuthModel;

namespace EssenceShop.Service.Interface
{
    public interface IAuthService
    {
        Task<BaseResponse<bool>> RegisterClients(RegisterUserDto request, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> LoginClients(LoginUserDto request, CancellationToken cancellationToken);
    }
}
