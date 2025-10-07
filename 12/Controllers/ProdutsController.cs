using Microsoft.AspNetCore.Mvc;
using EFCoreApp.Models;
using EFCoreApp.Repositories;
using EFCoreApp.DTOs;

namespace EFCoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            _logger.LogInformation("Запрос на получение всех продуктов");

            try
            {
                var products = await _productRepository.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении продуктов");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailDto>> GetProduct(int id)
        {
            _logger.LogInformation("Запрос на получение продукта с ID: {ProductId}", id);

            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Продукт с ID {ProductId} не найден", id);
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/products/category/1
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByCategory(int categoryId)
        {
            _logger.LogInformation("Запрос на получение продуктов категории: {CategoryId}", categoryId);

            try
            {
                var products = await _productRepository.GetProductsByCategoryAsync(categoryId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении продуктов категории {CategoryId}", categoryId);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        // GET: api/products/search/query
        [HttpGet("search/{query}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts(string query)
        {
            _logger.LogInformation("Поиск продуктов по запросу: {Query}", query);

            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Поисковый запрос не может быть пустым");
            }

            try
            {
                var products = await _productRepository.SearchProductsAsync(query);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске продуктов по запросу: {Query}", query);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDetailDto>> CreateProduct(Product product)
        {
            _logger.LogInformation("Создание нового продукта: {ProductName}", product.Name);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Некорректные данные продукта");
                return BadRequest(ModelState);
            }

            try
            {
                var createdProduct = await _productRepository.AddProductAsync(product);
                var productDto = await _productRepository.GetProductByIdAsync(createdProduct.Id);
                return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, productDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании продукта");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            _logger.LogInformation("Обновление продукта с ID: {ProductId}", id);

            if (id != product.Id)
            {
                _logger.LogWarning("Несоответствие ID в запросе: {RouteId} != {BodyId}", id, product.Id);
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Некорректные данные продукта для обновления");
                return BadRequest(ModelState);
            }

            try
            {
                var updatedProduct = await _productRepository.UpdateProductAsync(product);
                if (updatedProduct == null)
                {
                    _logger.LogWarning("Продукт с ID {ProductId} не найден для обновления", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении продукта с ID: {ProductId}", id);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            _logger.LogInformation("Удаление продукта с ID: {ProductId}", id);

            try
            {
                var result = await _productRepository.DeleteProductAsync(id);
                if (!result)
                {
                    _logger.LogWarning("Продукт с ID {ProductId} не найден для удаления", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении продукта с ID: {ProductId}", id);
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}