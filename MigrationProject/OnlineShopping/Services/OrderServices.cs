using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;
using Online.Contexts;
using Online.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Online.Services
{
    public class OrderService : IOrderService
    {
        private readonly MigrationContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserService _userService;
        private readonly IRepository<int, Product> _prodrepo;
        private readonly IRepository<int, Order> _orderrepo;
        private readonly IRepository<int, OrderDetail> _ODetailrepo;
        private readonly IRepository<int, Cart> _cartrepo;

        public OrderService(MigrationContext context, ICurrentUserService currentUserService, IUserService userService, IRepository<int, Product> prodrepo,
                                IRepository<int, Order> orderrepo, IRepository<int, OrderDetail> ODrepo, IRepository<int, Cart> cartrepo)
        {
            _context = context;
            _currentUser = currentUserService;
            _userService = userService;
            _prodrepo = prodrepo;
            _orderrepo = orderrepo;
            _ODetailrepo = ODrepo;
            _cartrepo = cartrepo;
        }

        public async Task<Order> PlaceOrder(IEnumerable<int> productIds)
        {
            double total = 0;
            bool available = true;
            var user = await _userService.GetByIdAsync(_currentUser.Id);
            var car = await _cartrepo.GetAllAsync();

            foreach (var id in productIds)
            {
                var product = await _prodrepo.GetByIdAsync(id);
                if (product == null || product.IsSold || product.IsSaleEnded)
                {
                    available = false;
                    break;
                }
                var qt = car.Where(c => c.ProductId == id).Sum(c => c.Quantity);
                total += product.Price * qt; //price as per quantity of per product!
            }

            if (!available)
            {
                throw new Exception("All products are not available");
            }
            else
            {
                Order ord = new();
                ord.OrderDate = DateTime.UtcNow;
                ord.TotalAmount = total;
                ord.UserId = user.UserId;
                ord.Status = "ORDERED";

                var ordered = await _orderrepo.AddAsync(ord);
                //var ordered = _context.Orders.Add(ord);
                await _context.SaveChangesAsync();

                foreach (var id in productIds)
                {
                    var product = await _prodrepo.GetByIdAsync(id);
                    var cart = _context.Carts.Where(c => c.ProductId == id && c.UserId == user.UserId).FirstOrDefault();
                    var orderdetail = new OrderDetail
                    {
                        OrderID = ordered.OrderId,
                        ProductID = product.ProductId,
                        Price = product.Price,
                        Quantity = cart.Quantity
                    };
                    //await _ODetailrepo.AddAsync(orderdetail);
                    _context.OrderDetails.Add(orderdetail);
                    await _context.SaveChangesAsync();

                    //removing product from cart after checking-out
                    var allcart = await _cartrepo.GetAllAsync();
                    var this_product = allcart.FirstOrDefault(c => c.UserId == user.UserId && c.ProductId == id);

                    if (this_product != null)
                    {
                        _context.Carts.Remove(this_product);
                        await _context.SaveChangesAsync();
                    }
                }
                return ordered;
            }
        }

        public async Task<IEnumerable<Order>> GetMyOrders()
        {
            var user = await _userService.GetByIdAsync(_currentUser.Id);

            var all_orders = await _orderrepo.GetAllAsync();

            var my_orders = all_orders.Where(o => o.UserId == user.UserId);

            return my_orders;
        }

        public async Task<IEnumerable<OrderDetail>> GetMyOrderDetails(int orderid)
        {
            var myorders = await GetMyOrders();

            var this_order = myorders.FirstOrDefault(o => o.OrderId == orderid);
            if (this_order == null)
            {
                throw new Exception("This order doesnt belong to the user!!!!");
            }

            var alldetails = await _ODetailrepo.GetAllAsync();
            var mydetails = alldetails.Where(o => o.OrderID == orderid);

            return mydetails;
        }


    }
    

        
}