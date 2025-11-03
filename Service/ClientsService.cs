using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto;
using EssenceShop.Dto.ClientsModel;
using EssenceShop.Repositries.Interface;
using EssenceShop.Service.Interface;

namespace EssenceShop.Service
{
    public class ClientsService(IClientsRepositries clientsRepositries, ILogger<ClientsService> logger, EssenceDbContext dbContext) : IClientsService
    {
        private readonly IClientsRepositries clientsRepositries = clientsRepositries;
        private readonly ILogger<ClientsService> logger = logger;
        private readonly EssenceDbContext dbContext = dbContext;

        public async Task<BaseResponse<Clients>> AddClients(CreateClientDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Clients>();

            try
            {
                logger.LogInformation("Registering client {Client}", request);
                var clients = new Clients
                {
                    ClientId = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    OtherName = request.OtherName,
                     Email = request.Email,
                    Address = request.Address

                };

                var saved = await clientsRepositries.AddClient(clients, cancellationToken);


                response.IsSuccess = true;
                response.Data = clients;
                response.Message = "Clients created successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error creating clients: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteClients(Guid Id, CancellationToken cancellationToken)
        {
            try
            {
                var isDeleted = await clientsRepositries.DeleteClients(Id, cancellationToken);
                if (!isDeleted)
                {
                    return new BaseResponse<bool>
                    {
                        IsSuccess = false,
                        Message = "Failed to delete clients",
                        Data = false
                    };
                }



                return new BaseResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Clients deleted successfully",
                    Data = true
                };


            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting usclientser");
                return new BaseResponse<bool>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the clients",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<List<Data.Clients>>> GetAllClients(CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<Data.Clients>>();

            try
            {
                var clothes = await clientsRepositries.GetAllClients(cancellationToken);


                response.IsSuccess = true;
                response.Data = clothes;
                response.Message = "Clients retrieved successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error retrieving client: {ex.Message}";
            }

            return response;
        }


        public async Task<BaseResponse<Clients>> GetClientsById(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Data.Clients>();

            try
            {
                var clients = await clientsRepositries.GetClientsById(id, cancellationToken);
                if (clients == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Clients not found";
                    return response;
                }

                response.IsSuccess = true;
                response.Data = clients;
                response.Message = "Client retrieved successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error retrieving clients: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<bool>> UpdateClients(Guid Id, UpdateClientsDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var clients = await clientsRepositries.GetClientsById(Id, cancellationToken);
                if (clients == null)
                {
                    response.IsSuccess = false;
                    response.Message = " clents not found";
                    return response;
                }

                clients.ClientId = Guid.NewGuid();
                clients.FirstName = request.FirstName;
                clients.OtherName = request.OtherName;
                clients.Address = request.Address;
                clients.Email = request.Email;




                response.IsSuccess = true;
                response.Data = true;
                response.Message = "Clients updated successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = false;
                response.Message = $"Error updating Clients: {ex.Message}";
            }

            return response;
        }
    }
}
