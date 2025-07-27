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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Attempting to fetch product with ID: {ProductId}", id);
            try
            {
                var product = await _productRepository.GetByIdAsync(id);

                if (product is null)
                {
                    _logger.LogWarning("Product with ID: {ProductId} not found.", id);
                    return null;
                }

                _logger.LogInformation("Successfully fetched product with ID: {ProductId}", id);
                return MapToDto(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching product with ID: {ProductId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            _logger.LogInformation("Attempting to fetch all products.");
            try
            {
                var allProducts = await _productRepository.GetAllAsync();
                _logger.LogInformation("Successfully fetched all products.");
                return allProducts.Select(MapToDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all products.");
                throw;
            }
        }

        public async Task<ProductDto> AddProductAsync(CreateProductDto createProductDto)
        {
            _logger.LogInformation("Attempting to add a new product with name: {ProductName}", createProductDto.Name);
            try
            {
                var productEntity = new Product
                {
                    Name = createProductDto.Name,
                    Price = createProductDto.Price,
                    Stock = createProductDto.Stock,
                    CategoryId = createProductDto.CategoryId
                };

                var newProductId = await _productRepository.AddAsync(productEntity);
                productEntity.Id = newProductId;

                _logger.LogInformation("Product created successfully with new ID: {NewProductId}", newProductId);
                return MapToDto(productEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new product. Details: {ProductDetails}", createProductDto);
                throw;
            }
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            _logger.LogInformation("Attempting to update product with ID: {ProductId}", id);
            try
            {
                var productToBeUpdated = await _productRepository.GetByIdAsync(id);

                if (productToBeUpdated is null)
                {
                    _logger.LogWarning("Attempted to update a non-existent product with ID: {ProductId}", id);
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(updateProductDto.Name))
                {
                    productToBeUpdated.Name = updateProductDto.Name;
                }
                if (updateProductDto.Price.HasValue)
                {
                    productToBeUpdated.Price = updateProductDto.Price.Value;
                }
                if (updateProductDto.Stock.HasValue)
                {
                    productToBeUpdated.Stock = updateProductDto.Stock.Value;
                }
                if (updateProductDto.CategoryId.HasValue)
                {
                    productToBeUpdated.CategoryId = updateProductDto.CategoryId.Value;
                }

                var updateResult = await _productRepository.UpdateAsync(productToBeUpdated);

                if (updateResult)
                    _logger.LogInformation("Product with ID: {ProductId} was updated successfully.", id);
                else
                    _logger.LogWarning("Failed to update product with ID: {ProductId} at the database level.", id);

                return updateResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating product with ID: {ProductId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            _logger.LogInformation("Attempting to delete product with ID: {ProductId}", id);
            try
            {
                var result = await _productRepository.DeleteAsync(id);

                if (result)
                    _logger.LogInformation("Product with ID: {ProductId} was deleted successfully.", id);
                else
                    _logger.LogWarning("Failed to delete product with ID: {ProductId}. It may not have existed.", id);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting product with ID: {ProductId}", id);
                throw;
            }
        }

        private ProductDto MapToDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            };
        }
    }
}