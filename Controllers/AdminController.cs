using EssenceShop.Context;
using EssenceShop.Dto.AuthModel;
using EssenceShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EssenceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class AdminController(EssenceDbContext dbContext, ILogger<AdminController> logger) : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger = logger;
        private readonly EssenceDbContext _dbContext = dbContext;




        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminLoginDto request,CancellationToken cancellationToken)
        {
            var response = await _adminService.RegisterAdmin(request,cancellationToken);
            return Ok(response);
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AdminLoginDto request,CancellationToken cancellationToken)
        {
            var response = await _adminService.Login(request,cancellationToken);
            return Ok(response);
        }
    }
}
