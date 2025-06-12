using Products.Common.Entities;
using Products.WebAPI.DTOs;

namespace Products.WebAPI.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponseDto ToResponseDto(Product product)
        {
            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description ?? "",
                Stock = product.Stock
            };
        }

        public static Product ToDomain(ProductDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Stock = productDto.Stock
            };
        }
    }
}
