using SimpleApi.Models.DTOS;

namespace SimpleApi.Services;

public class ProductService : IProductService
{
    private static readonly List<ProductDto> _products = new()
    {
        new ProductDto { Id = 1, Name = "Laptop", Price = 999.99M },
        new ProductDto { Id = 2, Name = "Headphones", Price = 199.99M }
    };

    public IEnumerable<ProductDto> GetAll() => _products;

    public ProductDto? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
}
