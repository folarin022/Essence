using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto;
using EssenceShop.Dto.ClothesModel;
using EssenceShop.Repositries.Interface;
using EssenceShop.Service.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EssenceShop.Service
{
    public class ClothesService : IClothesServices
    {
        private readonly IClothesRepositries _clothesRepository;
        private readonly ILogger<ClothesService> _logger;
        private readonly EssenceDbContext _dbContext;

        public ClothesService(IClothesRepositries clothesRepository, ILogger<ClothesService> logger, EssenceDbContext dbContext)
        {
            _clothesRepository = clothesRepository;
            _logger = logger;
            _dbContext = dbContext;
        }



        public async Task<BaseResponse<Clothes>> AddClothes(CreateClothesDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Clothes>();

            try
            {
                _logger.LogInformation("starting to add a new clothe {clothe}", request.ClientsName);

                var clothesAmount = new ClothesAmount { PricePerCloth = 400m };
                var total = request.Quantity * clothesAmount.PricePerCloth;

                var clothe = new Clothes
                {
                    ClientsName = request.ClientsName,
                    ClotheName = request.ClotheName,
                    ClotheColour = request.ClotheColour,
                    Quantity = request.Quantity,
                    AmountPaid = request.AmountPaid,
                    pricePerCloth = clothesAmount.PricePerCloth,
                    totalPrice = total,
                    Balance = total - request.AmountPaid
                };

                await _clothesRepository.AddClothes(clothe, cancellationToken);

                _logger.LogInformation(
                    "Clothe '{ClotheName}' added successfully for {ClientName}. Total: {Total}, Paid: {Paid}, Balance: {Balance}",
                    clothe.ClotheName, clothe.ClientsName, clothe.totalPrice, clothe.AmountPaid, clothe.Balance);

                response.IsSuccess = true; 
                response.Data = clothe;
                response.Message = "Clothe retrieved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding clothe for {ClientName}", request.ClientsName);
                response.IsSuccess = false;
                response.Message = $"Error creating clothes: {ex.Message}";
            }

            return response;
        }



        public async Task<BaseResponse<bool>> UpdateClothes(Guid id, UpdateClothesDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                _logger.LogInformation("Attempting to update clothe with ID {ClotheId}", id);

                var clothe = await _clothesRepository.GetClothesById(id, cancellationToken);
                if (clothe == null)
                {
                    _logger.LogWarning("Clothe with ID {ClotheId} not found for update", id);
                    response.IsSuccess = false;
                    response.Message = "Clothe not found.";
                    return response;
                }

                clothe.ClientsName = request.ClientsName;
                clothe.ClotheName = request.ClotheName;
                clothe.ClotheColour = request.ClotheColour;
                clothe.Quantity = request.Quantity;
                clothe.AmountPaid = request.AmountPaid;
                clothe.totalPrice = clothe.Quantity * clothe.pricePerCloth;
                clothe.Balance = clothe.totalPrice - clothe.AmountPaid;

                await _clothesRepository.SaveChangesAsync();

                _logger.LogInformation("Clothe {ClotheId} updated successfully", id);

                response.IsSuccess = true;
                response.Data = true;
                response.Message = "Clothe updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating clothe with ID {ClotheId}", id);
                response.IsSuccess = false;
                response.Data = false;
                response.Message = $"Error updating clothes: {ex.Message}";
            }

            return response;
        }



        public async Task<BaseResponse<List<Clothes>>> GetAllClothes(CancellationToken cancellationToken)
        {
            var response = new BaseResponse<List<Clothes>>();

            try
            {
                _logger.LogInformation("Fetching all clothes from the database...");

                var clothes = await _clothesRepository.GetAllClothes(cancellationToken);

                if (clothes == null || clothes.Count == 0)
                {
                    _logger.LogWarning("No clothes found in the database.");
                    response.IsSuccess = false;
                    response.Message = "No clothes found.";
                    return response;
                }

                _logger.LogInformation("{Count} clothes retrieved successfully", clothes.Count);

                response.IsSuccess = true;
                response.Data = clothes;
                response.Message = "Clothes retrieved successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all clothes from the database.");
                response.IsSuccess = false;
                response.Message = $"Error retrieving clothes: {ex.Message}";
            }

            return response;
        }



        public async Task<BaseResponse<Clothes>> GetClothesById(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Clothes>();

            try
            {
                _logger.LogInformation("Fetching clothe with ID {ClotheId}", id);

                var clothe = await _clothesRepository.GetClothesById(id, cancellationToken);
                if (clothe == null)
                {
                    _logger.LogWarning("Clothe with ID {ClotheId} not found", id);
                    response.IsSuccess = false;
                    response.Message = "Clothe not found.";
                    return response;
                }

                _logger.LogInformation("Clothe with ID {ClotheId} retrieved successfully", id);

                response.IsSuccess = true;
                response.Data = clothe;
                response.Message = "Clothe retrieved successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clothe with ID {ClotheId}", id);
                response.IsSuccess = false;
                response.Message = $"Error retrieving clothes: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<bool>> DeleteClothe(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                _logger.LogInformation("Attempting to delete clothe with ID {ClotheId}", id);

                var clothe = await _clothesRepository.GetClothesById(id, cancellationToken);
                if (clothe == null)
                {
                    _logger.LogWarning("Clothe with ID {ClotheId} not found for deletion", id);
                    response.IsSuccess = false;
                    response.Message = "Clothe not found.";
                    return response;
                }

                await _clothesRepository.DeleteClothes(id, cancellationToken);
                await _clothesRepository.SaveChangesAsync();

                _logger.LogInformation("Clothe with ID {ClotheId} deleted successfully", id);

                response.IsSuccess = true;
                response.Data = true;
                response.Message = "Clothe deleted successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting clothe with ID {ClotheId}", id);
                response.IsSuccess = false;
                response.Data = false;
                response.Message = $"Error deleting clothes: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<bool>> CollectClothe(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                _logger.LogInformation("Attempting to mark clothe {ClotheId} as collected", id);

                var isMarked = await _clothesRepository.CollectClothe(id, cancellationToken);

                if (!isMarked)
                {
                    _logger.LogWarning("Clothe with ID {ClotheId} not found or failed to mark as collected", id);
                    response.IsSuccess = false;
                    response.Message = "Clothe not found or failed to mark as collected.";
                    return response;
                }

                _logger.LogInformation("Clothe with ID {ClotheId} marked as collected successfully", id);

                response.IsSuccess = true;
                response.Message = "Clothe marked as collected successfully.";
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking clothe with ID {ClotheId} as collected", id);
                response.IsSuccess = false;
                response.Message = $"Error marking as collected: {ex.Message}";
            }

            return response;
        }
    }
}
