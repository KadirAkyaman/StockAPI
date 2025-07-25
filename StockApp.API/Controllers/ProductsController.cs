using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockApp.Core.DTOs;
using StockApp.Core.Interfaces;

namespace StockApp.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            var products = await _productService.GetAllProductsAsync();

            return Ok(products);
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProductAsync([FromBody] CreateProductDto createProductDto)
        {
            var createdProduct = await _productService.AddProductAsync(createProductDto);
            
            if (createdProduct == null)
                return StatusCode(500, "An error occurred while creating the product.");
            

            return StatusCode(201, createdProduct);
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            var res = await _productService.DeleteProductAsync(id);

            if (!res)
                return NotFound();
            
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProductAsync(int id, [FromBody] UpdateProductDto updateProductDto)
        {
            var res = await _productService.UpdateProductAsync(id, updateProductDto);
            if (!res)
                return NotFound();
            
            return NoContent();
        }
    }
}