using EssenceShop.Dto.ClientsModel;
using EssenceShop.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EssenceShop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }
        [Authorize (Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterClients(CreateClientDto input, CancellationToken cancellationToken)
        {
            await _clientsService.AddClients(input, cancellationToken);
            return Ok("clients registered successfully.");
        }
        [Authorize]
        [HttpGet("clients")]
        public async Task<IActionResult> GetAllClients(CancellationToken cancellationToken)
        {
            var response = await _clientsService.GetAllClients(cancellationToken);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-by-id{id:guid}")]
        public async Task<IActionResult> GetClientsById(Guid Id, CancellationToken cancellationToken )
        {
            var response = await _clientsService.GetClientsById(Id, cancellationToken);

            if (response == null)
            {
                return NotFound(new
                {
                    message = $"Clients with ID {Id} was not found."
                });
            }
            else
            {
                return Ok(response);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("update{id:guid}")]
        public async Task<IActionResult> UpdateClients(Guid Id,UpdateClientsDto request, CancellationToken cancellationToken)

        {
            var result = await _clientsService.UpdateClients(Id, request, cancellationToken);
            return Ok(result);
        }
        [Authorize (Roles = "Admin")]
        [HttpDelete("delete{id:guid}")]
        public async Task<IActionResult> DeleteClients(Guid Id, CancellationToken cancellationToken)
        {
            await _clientsService.DeleteClients(Id, cancellationToken);
            return Ok("Clients deleted successfully.");
        }
    }
}
