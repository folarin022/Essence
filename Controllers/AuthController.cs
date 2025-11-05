using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto.AuthModel;
using EssenceShop.Service;
using EssenceShop.Service.Interface;
using EssenceShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace EssenceShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request,CancellationToken cancellationToken)
        {
            var response = await _authService.RegisterClients(request, cancellationToken);
            return Ok(response);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto request,CancellationToken cancellationToken)
        {
            var response = await _authService.LoginClients(request, cancellationToken);
            return Ok(response);
        }


    }
}
