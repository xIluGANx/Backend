using Microsoft.EntityFrameworkCore;
using EFCoreApp.Models;
using EFCoreApp.Data;
using EFCoreApp.DTOs;
using EFCoreApp.Extensions;

namespace EFCoreApp.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            _logger.LogInformation("Получение всех продуктов");

            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .Select(p => p.ToDto()) // Преобразование в DTO
                .ToListAsync();
        }

        public async Task<ProductDetailDto?> GetProductByIdAsync(int id)
        {
            _logger.LogInformation("Получение продукта по ID: {ProductId}", id);

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            return product?.ToDetailDto(); // Преобразование в Detail DTO
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(int categoryId)
        {
            _logger.LogInformation("Получение продуктов по категории: {CategoryId}", categoryId);

            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .OrderBy(p => p.Name)
                .Select(p => p.ToDto()) // Преобразование в DTO
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            _logger.LogInformation("Поиск продуктов по запросу: {SearchTerm}", searchTerm);

            return await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive &&
                           (p.Name.Contains(searchTerm) ||
                            p.Description != null && p.Description.Contains(searchTerm)))
                .OrderBy(p => p.Name)
                .Select(p => p.ToDto()) // Преобразование в DTO
                .ToListAsync();
        }

        // Остальные методы остаются без изменений
        public async Task<Product> AddProductAsync(Product product)
        {
            _logger.LogInformation("Добавление нового продукта: {ProductName}", product.Name);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            _logger.LogInformation("Обновление продукта: {ProductId}", product.Id);

            var existingProduct = await _context.Products.FindAsync(product.Id);
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            _logger.LogInformation("Удаление продукта: {ProductId}", id);

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            product.IsActive = false;
            product.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}