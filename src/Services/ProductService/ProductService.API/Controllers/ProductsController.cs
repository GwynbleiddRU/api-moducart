using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductService.API.DTOs;
using ProductService.API.Models;
using ProductService.API.Repositories;

namespace ProductService.API.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductRepository productRepository,
            IMapper mapper,
            ILogger<ProductsController> logger
        )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products");
                return StatusCode(500, "An error occurred while retrieving products");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);

                if (product == null)
                    return NotFound();

                return Ok(_mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product {id}");
                return StatusCode(500, "An error occurred while retrieving the product");
            }
        }

        [HttpGet("category/{category}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(
            string category
        )
        {
            try
            {
                var products = await _productRepository.GetProductsByCategoryAsync(category);
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving products for category {category}");
                return StatusCode(500, "An error occurred while retrieving products by category");
            }
        }

        [HttpGet("featured")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetFeaturedProducts()
        {
            try
            {
                var products = await _productRepository.GetFeaturedProductsAsync();
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving featured products");
                return StatusCode(500, "An error occurred while retrieving featured products");
            }
        }

        [HttpGet("new-arrivals")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetNewArrivals()
        {
            try
            {
                var products = await _productRepository.GetNewArrivalsAsync();
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving new arrivals");
                return StatusCode(500, "An error occurred while retrieving new arrivals");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                product.CreatedAt = DateTime.UtcNow;

                var createdProduct = await _productRepository.AddProductAsync(product);
                return CreatedAtAction(
                    nameof(GetProduct),
                    new { id = createdProduct.Id },
                    _mapper.Map<ProductDto>(createdProduct)
                );
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return StatusCode(500, "An error occurred while creating the product");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id)
                return BadRequest("Product ID mismatch");

            try
            {
                var existingProduct = await _productRepository.GetProductByIdAsync(id);
                if (existingProduct == null)
                    return NotFound();

                _mapper.Map(productDto, existingProduct);
                await _productRepository.UpdateProductAsync(existingProduct);

                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product {id}");
                return StatusCode(500, "An error occurred while updating the product");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);

                if (product == null)
                    return NotFound();

                await _productRepository.DeleteProductAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return StatusCode(503, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product {id}");
                return StatusCode(500, "An error occurred while deleting the product");
            }
        }
    }
}
