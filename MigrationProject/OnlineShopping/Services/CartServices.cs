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
    public class CartService : ICartService
    {
        private readonly MigrationContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserService _userService;
        private readonly IRepository<int, Product> _prodrepo;
        private readonly IRepository<int, Cart> _cartrepo;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public CartService(MigrationContext context, ICurrentUserService currentUserService, IUserService userService, IRepository<int, Product> prodrepo, IRepository<int, Cart> cartrepo,
                                IOrderService orderService, IProductService productService)
        {
            _context = context;
            _currentUser = currentUserService;
            _userService = userService;
            _prodrepo = prodrepo;
            _cartrepo = cartrepo;
            _orderService = orderService;
            _productService = productService;
        }

        public async Task<Cart> AddtoCart(int productid)
        {
            var prod = await _productService.GetByIdAsync(productid);

            if (prod == null)
            {
                throw new Exception("Product not found!");
            }
            if (prod.IsSold)
            {
                throw new Exception("Product is sold!");
            }
            if (prod.IsSaleEnded)
            {
                throw new Exception("Product sale has ended!");
            }
            if (prod.UserId == _currentUser.Id)
            {
                throw new Exception("You cannot buy your own product.");
            }
            var user = await _userService.GetByIdAsync(_currentUser.Id);
            var cart = new Cart
            {
                ProductId = productid,
                UserId = user.UserId
            };
            var res = await _cartrepo.AddAsync(cart);
            await _context.SaveChangesAsync();
            return res;
        }

        public async Task<IEnumerable<Cart>> GetMyCart()
        {
            var user = await _userService.GetByIdAsync(_currentUser.Id);

            var cart = await _cartrepo.GetAllAsync();
            var mycart = cart.Where(x => x.UserId == user.UserId);

            return mycart;
        }

        public async Task<Order> BuyAllCart()
        {
            var mycart = await GetMyCart();
            List<int> prods = new List<int>();
            bool available = true;

            foreach (var car in mycart)
            {
                var prod = await _productService.GetByIdAsync(car.ProductId);
                if (prod == null || prod.IsSaleEnded || prod.IsSold)
                {
                    available = false;
                    break;
                }
                prods.Add(car.ProductId);
            }
            if (!available)
            {
                throw new Exception("Some of the items in your cart has been sold!");
            }
            else
            {
                var ordered = await _orderService.PlaceOrder(prods);
                return ordered;
            }
        }

        public async Task<Cart> RemoveItemFromCart(int productid)
        {
            var mycart = await GetMyCart();
            var this_product = mycart.FirstOrDefault(c => c.ProductId == productid);
            if (this_product == null)
            {
                throw new Exception("Product already does not exist!!!");
            }

            _context.Carts.Remove(this_product);
            await _context.SaveChangesAsync();

            return this_product;
        }

        public async Task<Order> BuySpecificItemFromCart(int productid)
        {
            var mycart = await GetMyCart();
            var this_product = mycart.FirstOrDefault(c => c.ProductId == productid);
            if (this_product == null)
            {
                throw new Exception("Product doesnt exist in your cart");
            }
            else
            {
                List<int> prods = new List<int>();
                prods.Add(this_product.ProductId);
                var ordered = await _orderService.PlaceOrder(prods);

                return ordered;
            }
        }
    }

}