using Azure.Core;
using EssenceShop.Data;
using EssenceShop.Dto;
using EssenceShop.Dto.ClothesModel;
using EssenceShop.Service;
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
            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Invalid clothe data"
                });

            var response = await _clothesService.AddClothes(input, cancellationToken);

            if (!response.IsSuccess)
                return BadRequest(response);

            return Ok(response);
        }


        [HttpGet("get-by-id/{id:guid}")]
        public async Task<IActionResult> GetClotheById(Guid id, CancellationToken cancellationToken)
        {
            var response = await _clothesService.GetClothesById(id, cancellationToken);

            if (response == null)
            {
                return NotFound(new
                {
                    message = $"Clients with ID {id} was not found."
                });
            }
            else
            {
                return Ok(response);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("collect/{id:guid}")]
        public async Task<IActionResult> CollectClothe(Guid id, CancellationToken cancellationToken)
        {
            var result = await _clothesService.CollectClothe(id, cancellationToken);
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("update/{id:guid}")]
        public async Task<IActionResult> UpdateClothe(Guid id, [FromBody] UpdateClothesDto request, CancellationToken cancellationToken)
        {
            var result = await _clothesService.UpdateClothes(id, request, cancellationToken);
            return Ok(result);
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllClothes(CancellationToken cancellationToken)
        {
            var response = await _clothesService.GetAllClothes(cancellationToken);
            return Ok(response);
        }



        [Authorize(Roles = "Admin")]
        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteClothe(Guid id, CancellationToken cancellationToken)
        {
            await _clothesService.DeleteClothe(id, cancellationToken);
            return Ok("Category deleted successfully.");
        }
    }
}
