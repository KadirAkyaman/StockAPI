using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockApp.Core.DTOs;

namespace StockApp.Core.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> AddProductAsync(CreateProductDto createProductDto);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
    }
}