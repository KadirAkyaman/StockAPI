using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockApp.Core.DTOs;
using StockApp.Core.Interfaces;

namespace StockApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryService categoryService, ILogger<CategoriesController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Executing GetAllCategoriesAsync: Fetching all categories.");
            var allCategories = await _categoryService.GetAllCategoryAsync();
            return Ok(allCategories);
        }

        [HttpGet("{id:int}", Name ="GetCategoryById")]
        public async Task<ActionResult<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            _logger.LogInformation("Executing GetCategoryByIdAsync for ID: {CategoryId}", id);
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category == null)
            {
                _logger.LogWarning("GetCategoryByIdAsync: Category with ID {CategoryId} not found.", id);
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategoryAsync([FromBody] CreateCategoryDto createCategoryDto)
        {
            _logger.LogInformation("Executing CreateCategoryAsync for category name: {CategoryName}", createCategoryDto.Name);
            var createdCategory = await _categoryService.AddCategoryAsync(createCategoryDto);

            if (createdCategory == null)
            {
                _logger.LogWarning("CreateCategoryAsync: Service failed to create category.");
                return StatusCode(500, "An error occurred while creating the category.");
            }

            _logger.LogInformation("CreateCategoryAsync: Successfully created category with ID {CategoryId}", createdCategory.Id);
             return CreatedAtAction("GetCategoryById", new { id = createdCategory.Id }, createdCategory);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategoryAsync(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            _logger.LogInformation("Executing UpdateCategoryAsync for ID: {CategoryId}", id);
            var result = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);

            if (!result)
            {
                _logger.LogWarning("UpdateCategoryAsync: Update failed for ID {CategoryId}. Category may not exist.", id);
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategoryAsync(int id)
        {
            _logger.LogInformation("Executing DeleteCategoryAsync for ID: {CategoryId}", id);
            var result = await _categoryService.DeleteCategoryAsync(id);

            if (!result)
            {
                _logger.LogWarning("DeleteCategoryAsync: Delete failed for ID {CategoryId}. Category may not exist.", id);
                return NotFound();
            }
            
            return NoContent();
        }
    }
}