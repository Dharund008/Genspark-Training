using SimpleApi.Models.DTOS;

namespace SimpleApi.Services;

public interface IProductService
{
    IEnumerable<ProductDto> GetAll();
    ProductDto? GetById(int id);
}