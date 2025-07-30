
using Online.Models;
namespace Online.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrder(IEnumerable<int> product_Ids);
        Task<IEnumerable<Order>> GetMyOrders();
        Task<IEnumerable<OrderDetail>> GetMyOrderDetails(int orderid);
    }
}