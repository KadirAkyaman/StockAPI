using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StockApp.Core.DTOs;
using StockApp.Core.Interfaces;
using StockApp.Core.Models;

namespace StockApp.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            _logger.LogInformation("Attempting to fetch category with ID: {CategoryId}", id);
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);

                if (category is null)
                {
                    _logger.LogWarning("Category with ID: {CategoryId} not found.", id);
                    return null;
                }

                _logger.LogInformation("Successfully fetched category with ID: {CategoryId}", id);
                return MapToDto(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching category with ID: {CategoryId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoryAsync()
        {
            _logger.LogInformation("Attempting to fetch all categories.");
            try
            {
                var allCategories = await _categoryRepository.GetAllAsync();
                _logger.LogInformation("Successfully fetched all categories.");
                return allCategories.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all categories.");
                throw;
            }
        }

        public async Task<CategoryDto> AddCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            _logger.LogInformation("Attempting to add a new category with name: {CategoryName}", createCategoryDto.Name);
            try
            {
                var category = new Category
                {
                    Name = createCategoryDto.Name
                };

                var newCategoryId = await _categoryRepository.AddAsync(category);
                category.Id = newCategoryId;
                
                _logger.LogInformation("Category created successfully with new ID: {NewCategoryId}", newCategoryId);
                return MapToDto(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new category. Details: {CategoryDetails}", createCategoryDto);
                throw;
            }
        }
        
        public async Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            _logger.LogInformation("Attempting to update category with ID: {CategoryId}", id);
            try
            {
                var categoryToBeUpdated = await _categoryRepository.GetByIdAsync(id);

                if (categoryToBeUpdated is null)
                {
                    _logger.LogWarning("Attempted to update a non-existent category with ID: {CategoryId}", id);
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(updateCategoryDto.Name))
                {
                    categoryToBeUpdated.Name = updateCategoryDto.Name;
                }

                var updateResult = await _categoryRepository.UpdateAsync(categoryToBeUpdated);

                if(updateResult)
                    _logger.LogInformation("Category with ID: {CategoryId} was updated successfully.", id);
                else
                    _logger.LogWarning("Failed to update category with ID: {CategoryId} at the database level.", id);
                
                return updateResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating category with ID: {CategoryId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            _logger.LogInformation("Attempting to delete category with ID: {CategoryId}", id);
            try
            {
                var result = await _categoryRepository.DeleteAsync(id);

                if (result)
                    _logger.LogInformation("Category with ID: {CategoryId} was deleted successfully.", id);
                else
                    _logger.LogWarning("Failed to delete category with ID: {CategoryId}. It may not have existed.", id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting category with ID: {CategoryId}", id);
                throw;
            }
        }

        private CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}