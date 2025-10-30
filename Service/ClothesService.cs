using Azure;
using EssenceShop.Context;
using EssenceShop.Data;
using EssenceShop.Dto;
using EssenceShop.Dto.ClothesModel;
using EssenceShop.Repositries.Interface;
using EssenceShop.Service.Interface;
using Microsoft.Extensions.Logging;

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


                var saved = await _clothesRepository.AddClothes(clothe, cancellationToken);

                response.IsSuccess = true;
                response.Data = clothe;
                response.Message = "Clothes created successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating clothes");
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error creating clothes: {ex.Message}";
            }

            return response;
        }

        public async Task<BaseResponse<bool>> UpdateClothes(Guid id, UpdateClothesDto request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var clothe = await _clothesRepository.GetClothesById(id, cancellationToken);
                if (clothe == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Clothes not found";
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

                response.IsSuccess = true;
                response.Data = true;
                response.Message = "Clothes updated successfully";
            }
            catch (Exception ex)
            {
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
                var clothes = await _clothesRepository.GetAllClothes(cancellationToken);
                response.IsSuccess = true;
                response.Data = clothes;
                response.Message = "Clothes retrieved successfully";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error retrieving clothes: {ex.Message}";
            }

            return response;
        }
        public async Task<BaseResponse<Clothes>> GetClothesById(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<Clothes>();

            try
            {
                var clothe = await _clothesRepository.GetClothesById(id, cancellationToken);
                if (clothe == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Clothes not found";
                    return response;
                }

                response.IsSuccess = true;
                response.Data = clothe;
                response.Message = "Clothes retrieved successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving clothes");
                response.IsSuccess = false;
                response.Data = null;
                response.Message = $"Error retrieving clothes: {ex.Message}";
            }

            return response;
        }


        public async Task<BaseResponse<bool>> DeleteClothe(Guid id, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var clothe = await _clothesRepository.GetClothesById(id, cancellationToken);
                if (clothe == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Clothes not found";
                    response.Data = false;
                    return response;
                }

                await _clothesRepository.DeleteClothes(id, cancellationToken);
                await _clothesRepository.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "Clothes deleted successfully";
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting clothes");
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
                var isMarked = await _clothesRepository.CollectClothe(id, cancellationToken);

                if (!isMarked)
                {
                    response.IsSuccess = false;
                    response.Message = "Clothe not found or failed to mark as collected.";
                    response.Data = false;
                    return response;
                }

                response.IsSuccess = true;
                response.Message = "Clothe marked as collected successfully.";
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking clothe as collected");
                response.IsSuccess = false;
                response.Message = $"Error marking as collected: {ex.Message}";
            }

            return response;
        }

    }
}
    
            
            
              
            


           