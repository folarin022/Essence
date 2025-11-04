using EssenceShop.Dto;
using EssenceShop.Dto.ClientsModel;
using EssenceShop.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EssenceShop.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IClientsService clientsService, ILogger<ClientsController> logger)
        {
            _clientsService = clientsService;
            _logger = logger;
        }





        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RegisterClients(CreateClientDto input, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Invalid product data"
                });

            var response = await _clientsService.AddClients(input, cancellationToken);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }



        [Authorize]
        [HttpGet("clients")]
        public async Task<IActionResult> GetAllClients(CancellationToken cancellationToken)
        {
            var response = await _clientsService.GetAllClients(cancellationToken);
            return Ok(response);

        }


        [Authorize]
        [HttpGet("get-by-id/{id:guid}")]
        public async Task<IActionResult> GetClientsById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _clientsService.GetClientsById(id, cancellationToken);

            if (response == null)
            {
                return NotFound(new
                {
                    message = $"Product with ID {id} was not found."
                });
            }
            else
            {
                return Ok(response);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update{id:guid}")]
        public async Task<IActionResult> UpdateClients(Guid Id, UpdateClientsDto request, CancellationToken cancellationToken)
        {
            var result = await _clientsService.UpdateClients(Id, request, cancellationToken);
            return Ok(result);
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("delete{id:guid}")]
        public async Task<IActionResult> DeleteClients(Guid Id, CancellationToken cancellationToken)
        {
            await _clientsService.DeleteClients(Id, cancellationToken);
            return Ok("Category deleted successfully.");
        }

    }
}
