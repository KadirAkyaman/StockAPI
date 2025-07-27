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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            _logger.LogInformation("Executing GetAllProductsAsync: Fetching all products.");
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}", Name ="GetProductById")]
        public async Task<ActionResult<ProductDto>> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Executing GetProductByIdAsync for ID: {ProductId}", id);
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("GetProductByIdAsync: Product with ID {ProductId} not found.", id);
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync([FromBody] CreateProductDto createProductDto)
        {
            _logger.LogInformation("Executing CreateProductAsync for product name: {ProductName}", createProductDto.Name);
            var createdProduct = await _productService.AddProductAsync(createProductDto);

            if (createdProduct == null)
            {
                _logger.LogWarning("CreateProductAsync: Service failed to create product.");
                // Not: Orijinal koddaki "pruduct" yazım hatası düzeltildi.
                return StatusCode(500, "An error occurred while creating the product.");
            }

            _logger.LogInformation("CreateProductAsync: Successfully created product with ID {ProductId}", createdProduct.Id);
            return CreatedAtAction("GetProductById", new { id = createdProduct.Id }, createdProduct);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            _logger.LogInformation("Executing DeleteProductAsync for ID: {ProductId}", id);
            var result = await _productService.DeleteProductAsync(id);

            if (!result)
            {
                _logger.LogWarning("DeleteProductAsync: Delete failed for ID {ProductId}. Product may not exist.", id);
                return NotFound();
            }

            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProductAsync(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            _logger.LogInformation("Executing UpdateProductAsync for ID: {ProductId}", id);
            var result = await _productService.UpdateProductAsync(id, updateProductDto);
            
            if (!result)
            {
                _logger.LogWarning("UpdateProductAsync: Update failed for ID {ProductId}. Product may not exist.", id);
                return NotFound();
            }

            return NoContent();
        }
    }
}