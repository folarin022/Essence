using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto;
using EssenceShop.Dto.ClientsModel;
using EssenceShop.Repositries.Interface;
using EssenceShop.Service.Interface;
using EssenceShop.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EssenceShop.Service
{
    public class ClientsService : IClientsService
    {
        private readonly IAdminRepositries _clientsRepositries;
        private readonly ILogger<ClientsService> _logger;
        private readonly EssenceDbContext _dbContext;

        public ClientsService(
            IAdminRepositries clientsRepositries,
            ILogger<ClientsService> logger,
            EssenceDbContext dbContext)
        {
            _clientsRepositries = clientsRepositries;
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task<BaseResponse<Clients>> AddClients(CreateClientDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Clients>();

            try
            {
                _logger.LogInformation("Attempting to create client with email: {Email}", request.Email);

                if (await _dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
                {
                    _logger.LogWarning("Client creation failed: Email {Email} already exists", request.Email);
                    response.IsSuccess = false;
                    response.Message = "Email already exists.";
                    return response;
                }

                if (!ValidationService.IsValidEmail(request.Email))
                {
                    _logger.LogWarning("Invalid email format for client: {Email}", request.Email);
                    response.IsSuccess = false;
                    response.Message = "Invalid email format.";
                    return response;
                }

                var client = new Clients
                {
                    ClientId = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    OtherName = request.OtherName,
                    Email = request.Email,
                    Address = request.Address
                };

                await _clientsRepositries.AddClient(client, cancellationToken);

                _logger.LogInformation("Client {Email} created successfully with ID {ClientId}", client.Email, client.ClientId);

                response.IsSuccess = true;
                response.Data = client;
                response.Message = "Client created successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating client with email {Email}", request.Email);
                response.IsSuccess = false;
                response.Message = $"Error creating client: {ex.Message}";
            }

            return response;
        }


        public async Task<BaseResponse<List<Clients>>> GetAllClients(CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<Clients>>();

            try
            {
                _logger.LogInformation("Fetching all clients from database");

                var clients = await _clientsRepositries.GetAllClients(cancellationToken);

                if (clients == null || !clients.Any())
                {
                    _logger.LogWarning("No clients found in database");
                    response.IsSuccess = false;
                    response.Message = "No clients found";
                    return response;
                }

                _logger.LogInformation("{Count} clients retrieved successfully", clients.Count);

                response.IsSuccess = true;
                response.Data = clients;
                response.Message = "Clients retrieved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving clients");
                response.IsSuccess = false;
                response.Message = $"Error retrieving clients: {ex.Message}";
            }

            return response;
        }


        public async Task<BaseResponse<Clients>> GetClientsById(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Clients>();

            try
            {
                _logger.LogInformation("Fetching client with ID: {ClientId}", id);

                var client = await _clientsRepositries.GetClientsById(id, cancellationToken);

                if (client == null)
                {
                    _logger.LogWarning("Client with ID {ClientId} not found", id);
                    response.IsSuccess = false;
                    response.Message = "Client not found";
                    return response;
                }

                _logger.LogInformation("Client {ClientId} retrieved successfully", id);

                response.IsSuccess = true;
                response.Data = client;
                response.Message = "Client retrieved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving client with ID {ClientId}", id);
                response.IsSuccess = false;
                response.Message = $"Error retrieving client: {ex.Message}";
            }

            return response;
        }


        public async Task<BaseResponse<bool>> UpdateClients(Guid id, UpdateClientsDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                _logger.LogInformation("Attempting to update client with ID: {ClientId}", id);

                var client = await _clientsRepositries.GetClientsById(id, cancellationToken);
                if (client == null)
                {
                    _logger.LogWarning("Client with ID {ClientId} not found for update", id);
                    response.IsSuccess = false;
                    response.Message = "Client not found";
                    return response;
                }

                client.FirstName = request.FirstName;
                client.OtherName = request.OtherName;
                client.Email = request.Email;
                client.Address = request.Address;

                await _clientsRepositries.UpdateClientsDto(id,request, cancellationToken);

                _logger.LogInformation("Client {ClientId} updated successfully", id);

                response.IsSuccess = true;
                response.Data = true;
                response.Message = "Client updated successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating client with ID {ClientId}", id);
                response.IsSuccess = false;
                response.Data = false;
                response.Message = $"Error updating client: {ex.Message}";
            }

            return response;
        }



        public async Task<BaseResponse<bool>> DeleteClients(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                _logger.LogInformation("Attempting to delete client with ID: {ClientId}", id);

                var isDeleted = await _clientsRepositries.DeleteClients(id, cancellationToken);
                if (!isDeleted)
                {
                    _logger.LogWarning("Failed to delete client with ID: {ClientId}", id);
                    response.IsSuccess = false;
                    response.Data = false;
                    response.Message = "Failed to delete client";
                    return response;
                }

                _logger.LogInformation("Client {ClientId} deleted successfully", id);

                response.IsSuccess = true;
                response.Data = true;
                response.Message = "Client deleted successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting client {ClientId}", id);
                response.IsSuccess = false;
                response.Data = false;
                response.Message = $"Error deleting client: {ex.Message}";
            }

            return response;
        }
    }
}
