using hoangngocthe_2123110488.DTOs;
using hoangngocthe_2123110488.Service;
using Microsoft.AspNetCore.Mvc;

namespace hoangngocthe_2123110488.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoriesController(ICategoryService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetCategories() => Ok(await _service.GetAllCategoriesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await _service.GetCategoryByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            try
            {
                var result = await _service.CreateCategoryAsync(dto);
                return CreatedAtAction(nameof(GetCategory), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreateCategoryDto dto)
        {
            var success = await _service.UpdateCategoryAsync(id, dto);
            return success ? Ok(new { message = "Cập nhật thành công" }) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var success = await _service.DeleteCategoryAsync(id);
                return success ? Ok(new { message = "Xóa thành công" }) : NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
