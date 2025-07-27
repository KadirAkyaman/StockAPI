using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockApp.Core.DTOs;
using StockApp.Core.Models;

namespace StockApp.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategoryAsync();
        Task<CategoryDto> AddCategoryAsync(CreateCategoryDto createCategoryDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
    }
}