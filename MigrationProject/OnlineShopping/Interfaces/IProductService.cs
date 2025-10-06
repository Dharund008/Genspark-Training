
using Online.Models;
using Online.Models.DTO;

namespace Online.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductByName(string ProductName);
        Task<Product> GetSoldProductByName(string ProductName);
        Task<Product> GetByIdAsync(int Id);

        Task<Product> AddProduct(AddprdDTO request);
        Task<Product> ExtendSale(ExtendSaleDTO sale);

        //Task<Product> UpdateProduct(Product request);
        Task<bool> UpdateSold(int productID);
        Task<Product> UpdatePrice(PriceDTO price);
        Task<Product> DeleteProduct(int id,string name);

        Task<IEnumerable<Product>> MyProducts();

        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetAllProductAsync();
        Task<IEnumerable<Product>> GetFilteredProducts(string cat, string color, string model, string prodname);

    }
}