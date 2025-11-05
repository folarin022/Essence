using EssenceShop.Dto.AuthModel;

namespace EssenceShop.Repositries.Interface
{
    public interface IAdminRepository
    {
        Task<bool> RegisterAdmin(AdminLoginDto request,CancellationToken cancellationToken);
        Task<bool> Login(AdminLoginDto request,CancellationToken cancellationToken);
    }
}
