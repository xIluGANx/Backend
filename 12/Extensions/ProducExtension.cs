using EFCoreApp.Models;
using EFCoreApp.DTOs;

namespace EFCoreApp.Extensions
{
    public static class ProductExtensions
    {
        public static ProductDto ToDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate,
                IsActive = product.IsActive
            };
        }

        public static ProductDetailDto ToDetailDto(this Product product)
        {
            return new ProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Category = product.Category?.ToDto(),
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate,
                IsActive = product.IsActive
            };
        }
    }

    public static class CategoryExtensions
    {
        public static CategoryDto ToDto(this Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate
            };
        }
    }
}