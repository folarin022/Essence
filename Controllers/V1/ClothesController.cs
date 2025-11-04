using EssenceShop.Data;
using EssenceShop.Dto.ClothesModel;
using EssenceShop.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EssenceShop.Controllers.V1
{
        [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    
    public class ClothesController : ControllerBase
    {
        private readonly IClothesServices _clothesService;
        private readonly ILogger<ClothesController> _logger;

        public ClothesController(IClothesServices clothesService, ILogger<ClothesController> logger)
        {
            _clothesService = clothesService;
            _logger = logger;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddClothe([FromBody] CreateClothesDto input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin {User} is adding a new clothe: {ClotheName}",
                User.Identity?.Name, input.ClotheName);

            try
            {
                var result = await _clothesService.AddClothes(input, cancellationToken);
                _logger.LogInformation("Clothe {ClotheName} added successfully by {Admin}",
                    input.ClotheName, User.Identity?.Name);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding clothe {ClotheName}", input.ClotheName);
                return StatusCode(500, "An error occurred while adding the clothe.");
            }
        }


        [HttpGet("get-by-id/{id:guid}")]
        public async Task<IActionResult> GetClotheById(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {User} requested clothe details for ID {ClotheId}",
                User.Identity?.Name, id);

            try
            {
                var response = await _clothesService.GetClothesById(id, cancellationToken);

                if (response == null)
                {
                    _logger.LogWarning("Clothe with ID {ClotheId} not found. Requested by {User}",
                        id, User.Identity?.Name);
                    return NotFound(new { message = $"Clothe with ID {id} was not found." });
                }

                _logger.LogInformation("Clothe with ID {ClotheId} retrieved successfully by {User}",
                    id, User.Identity?.Name);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving clothe with ID {ClotheId}", id);
                return StatusCode(500, "An error occurred while fetching the clothe.");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("collect/{id:guid}")]
        public async Task<IActionResult> CollectClothe(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin {Admin} is collecting clothe with ID {ClotheId}",
                User.Identity?.Name, id);

            var response = await _clothesService.CollectClothe(id, cancellationToken);
            if (!response.IsSuccess)
            {
                _logger.LogWarning("Failed to collect clothe with ID {ClotheId}: {Message}",
                    id, response.Message);
                return BadRequest(response);
            }

            _logger.LogInformation("Clothe with ID {ClotheId} collected successfully", id);
            return Ok(response);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateClothe(Guid id, [FromBody] UpdateClothesDto request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin {Admin} is updating clothe with ID {ClotheId}",
                User.Identity?.Name, id);

            var result = await _clothesService.UpdateClothes(id, request, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Update failed for clothe {ClotheId}: {Message}", id, result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Clothe with ID {ClotheId} updated successfully", id);
            return Ok(result);
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllClothes(CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {User} requested all clothes", User.Identity?.Name);

            var response = await _clothesService.GetAllClothes(cancellationToken);
            _logger.LogInformation("Fetched {Count} clothes successfully", response.Data?.Count ?? 0);

            return Ok(response);
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteClothe(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Admin {Admin} is deleting clothe with ID {ClotheId}",
                User.Identity?.Name, id);

            await _clothesService.DeleteClothe(id, cancellationToken);

            _logger.LogInformation("Clothe with ID {ClotheId} deleted successfully", id);
            return Ok("Clothe deleted successfully.");
        }
    }
}
