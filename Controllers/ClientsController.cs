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
            _logger.LogInformation("Admin {User} is registering a new client with name {ClientName}",
                User.Identity?.Name, input.FirstName);

            try
            {
                await _clientsService.AddClients(input, cancellationToken);
                _logger.LogInformation("Client {ClientName} registered successfully by {User}",
                    input.FirstName, User.Identity?.Name);

                return Ok("Client registered successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while registering client {ClientName}", input.FirstName);
                return StatusCode(500, "An error occurred while registering the client.");
            }
        }



        [Authorize]
        [HttpGet("clients")]
        public async Task<IActionResult> GetAllClients(CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {User} requested all clients", User.Identity?.Name);

            var response = await _clientsService.GetAllClients(cancellationToken);

            _logger.LogInformation("Fetched {Count} clients successfully", response.Data.Count);
            return Ok(response);

        }


        [Authorize]
        [HttpGet("get-by-id/{id:guid}")]
        public async Task<IActionResult> GetClientsById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin {User} requested client details for ID {ClientId}",
                User.Identity?.Name, id);

            try
            {
                var response = await _clientsService.GetClientsById(id, cancellationToken);

                if (response == null)
                {
                    _logger.LogWarning("Client with ID {ClientId} not found. Requested by {User}",
                        id, User.Identity?.Name);
                    return NotFound(new { message = $"Client with ID {id} was not found." });
                }

                _logger.LogInformation("Client with ID {ClientId} retrieved successfully by {User}",
                    id, User.Identity?.Name);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving client with ID {ClientId} by {User}",
                    id, User.Identity?.Name);
                return StatusCode(500, "An error occurred while fetching the client.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update{id:guid}")]
        public async Task<IActionResult> UpdateClients(Guid Id, UpdateClientsDto request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin {User} attempting to update client with ID {ClientId}",
                User.Identity?.Name, Id);

            try
            {
                var result = await _clientsService.UpdateClients(Id, request, cancellationToken);

                if (result == null)
                {
                    _logger.LogWarning("Client with ID {ClientId} not found for update by {User}",
                        Id, User.Identity?.Name);
                    return NotFound(new { message = $"Client with ID {Id} not found." });
                }

                _logger.LogInformation("Client with ID {ClientId} updated successfully by {User}",
                    Id, User.Identity?.Name);

                return Ok(new { message = "Client updated successfully.", data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating client with ID {ClientId} by {User}",
                    Id, User.Identity?.Name);
                return StatusCode(500, "An error occurred while updating the client.");
            }
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("delete{id:guid}")]
        public async Task<IActionResult> DeleteClients(Guid Id, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Admin {User} attempting to delete client with ID {ClientId}",
                User.Identity?.Name, Id);

            await _clientsService.DeleteClients(Id, cancellationToken);

            _logger.LogInformation("Client with ID {ClientId} deleted successfully by {User}",
                Id, User.Identity?.Name);

            return Ok("Client deleted successfully.");
        }

    }
}
