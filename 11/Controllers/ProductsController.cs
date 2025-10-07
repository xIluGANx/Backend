using Microsoft.AspNetCore.Mvc;
using WebAPIApp.Models;
using WebAPIApp.Repositories;

namespace WebAPIApp.Controllers
{
    /// <summary>
    /// Контроллер для управления товарами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Конструктор с внедрением зависимости репозитория
        /// </summary>
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// GET: api/products
        /// Получение всех товаров
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _productRepository.GetAll();
            return Ok(products);
        }

        /// <summary>
        /// GET: api/products/{id}
        /// Получение товара по ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return NotFound($"Товар с ID {id} не найден");
            }

            return Ok(product);
        }

        /// <summary>
        /// POST: api/products
        /// Создание нового товара
        /// </summary>
        [HttpPost]
        public ActionResult<Product> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productRepository.Add(product);

            // Возвращаем созданный товар с кодом 201
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        /// <summary>
        /// PUT: api/products/{id}
        /// Обновление существующего товара
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("ID в пути не совпадает с ID в теле запроса");
            }

            if (!_productRepository.Exists(id))
            {
                return NotFound($"Товар с ID {id} не найден");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _productRepository.Update(product);

            return NoContent(); // 204 No Content
        }

        /// <summary>
        /// DELETE: api/products/{id}
        /// Удаление товара
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            if (!_productRepository.Exists(id))
            {
                return NotFound($"Товар с ID {id} не найден");
            }

            _productRepository.Delete(id);

            return NoContent(); // 204 No Content
        }
    }
}