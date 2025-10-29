using EssenceShop.Data;
using EssenceShop.Dto.ClothesModel;
using EssenceShop.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace EssenceShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClothesController : ControllerBase
    {

        private readonly IClothesServices _clothesService;

        public ClothesController(IClothesServices clothesService)
        {
            _clothesService = clothesService;
        }


        [HttpPost]
        public async Task<IActionResult> AddClothe(CreateClothesDto request,CancellationToken cancellationToken)
        {
            await _clothesService.AddClothes(request, cancellationToken);
            return Ok("Clothes added successfully.");
        }

        [HttpGet("Id:{id:guid}")]
        public async Task<IActionResult> GetClotheByID(Guid id, CancellationToken cancellationToken)
        {
            var response = await _clothesService.GetClothesById(id, cancellationToken);

            if (response == null)
            {
                return NotFound(new
                {
                    message = $"Clothes with ID {id} was not found."
                });
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut("update{id:guid}")]
        public async Task<IActionResult> UpdateClothe(Guid Id,UpdateClothesDto request,CancellationToken cancellationToken)
        {
            var result = await _clothesService.UpdateClothes(Id,request,cancellationToken);
            return Ok(result);
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllClothes (CancellationToken cancellationToken)
        {
            var response = await _clothesService.GetAllClothes(cancellationToken);
            return Ok(response);
        }

        [HttpDelete("delete{id:guid}")]
        public async Task<IActionResult> DeleteClothe(Guid id, CancellationToken cancellationToken )
        {
            await _clothesService.DeleteClothe(id, cancellationToken);
            return Ok("Clothes deleted successfully.");
        }
    }
}
