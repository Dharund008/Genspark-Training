
using Online.Models;

namespace Online.Interfaces
{
    public interface ICartService
    {
        Task<Cart> AddtoCart(int productid, int quantity);
        Task<IEnumerable<Cart>> GetMyCart();
        Task<Order> BuyAllCart();
        Task<Order> BuySpecificItemFromCart(int productid);
        Task<Cart> RemoveItemFromCart(int productid);
    }
}