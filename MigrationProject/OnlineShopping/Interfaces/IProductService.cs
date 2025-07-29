
using Online.Models;
using Online.Models.DTO;

namespace Online.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductByName(string ProductName);
        Task<Product> GetByIdAsync(int Id);

        Task<Product> AddProduct(Product request);

        Task<Product> UpdateProduct(Product request);

        Task<Product> DeleteProduct(Product request);

        Task<IEnumerable<Product>> GetAllAsync();

    }
}