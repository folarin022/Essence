using EssenceShop.Dto;
using EssenceShop.Dto.AuthModel;

namespace EssenceShop.Service.Interface
{
    public interface IAdminService
    {
        Task<BaseResponse<bool>> RegisterAdmin(AdminLoginDto request, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> Login(AdminLoginDto request, CancellationToken cancellationToken);
    }
}
