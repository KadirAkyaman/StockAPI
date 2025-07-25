using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewAPI.Repositories;
using StockApp.Core.DTOs;
using StockApp.Core.Interfaces;
using StockApp.Core.Models;
using StockApp.Core.Repositories;

namespace StockApp.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDto> AddProductAsync(CreateProductDto createProductDto)
        {
            var prod = new Product
            {
                Name = createProductDto.Name,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                CategoryId = createProductDto.CategoryId
            };

            var newProdId = await _productRepository.AddAsync(prod);

            var productDto = new ProductDto
            {
                Name = prod.Name,
                Price = prod.Price,
                Stock = prod.Stock,
                CategoryId = prod.CategoryId
            };
            
            return productDto;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var allProducts = await _productRepository.GetAllAsync();

            return allProducts.Select(prod => new ProductDto
            {
                Name = prod.Name,
                Price = prod.Price,
                Stock = prod.Stock,
                CategoryId = prod.CategoryId
            });
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product is null)
                return null;


            var productDto = new ProductDto
            {
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            };

            return productDto;
        }

        public async Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
        {
            var productToBeUpdated = await _productRepository.GetByIdAsync(id);

            if (productToBeUpdated is null)
            {
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

            return await _productRepository.UpdateAsync(productToBeUpdated);
        }
    }
}