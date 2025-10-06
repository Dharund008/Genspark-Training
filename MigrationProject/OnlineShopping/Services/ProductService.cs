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
    public class ProductService : IProductService
    {
        private readonly MigrationContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IFunctionServices _otherService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<int, Product> _prodrepo;

        public ProductService(IRepository<int, Product> prodrepo, MigrationContext context, IHttpContextAccessor httpContextAccessor,
                                 IFunctionServices otherService, IUserService userService, ICurrentUserService currentUser)
        {
            _prodrepo = prodrepo;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _otherService = otherService;
            _userService = userService;
            _currentUser = currentUser;
        }

        public async Task<Product> GetProductByName(string productname) //unsold
        {
            productname = productname.ToLower();
            var prod = await _prodrepo.GetAllAsync();

            var product = prod.FirstOrDefault(c => c.ProductName.ToLower() == productname && c.IsSold == false);
            if (product == null)
            {
                throw new Exception("Product not found!");
            }
            return product;
        }

        public async Task<Product> GetSoldProductByName(string productname)
        {
            productname = productname.ToLower();
            var prod = await _prodrepo.GetAllAsync();
            var product = prod.FirstOrDefault(c => c.ProductName.ToLower() == productname && c.IsSold);
            if (product == null)
            {
                throw new Exception("Product not found!");
            }
            return product;
        }


        public async Task<Product> GetByIdAsync(int Id)
        {
            if (Id == 0)
            {
                throw new Exception("Id not valid!");
            }
            var prod = await _prodrepo.GetByIdAsync(Id);
            if (prod == null)
            {
                throw new Exception("Product not exists!");
            }
            if (prod.IsSold)
            {
                throw new Exception("Product is sold!");
            }
            return prod;
        }

        public async Task<Product> AddProduct(AddprdDTO request)
        {
            var category = await _otherService.GetCategoryByName(request.Category.ToLower());
            var color = await _otherService.GetColorByName(request.Color.ToLower());
            var model = await _otherService.GetModelByName(request.Model.ToLower());

            if (request == null || category == null || color == null || model == null)
            {
                throw new Exception("Request is null!");
            }
            var currentUser = _httpContextAccessor.HttpContext?.User.FindFirst("MyApp_Id")?.Value;
            var user = await _userService.GetByIdAsync(int.Parse(currentUser));
            var prod = new Product
            {
                ProductName = request.ProductName,
                Price = request.Price,
                Image = request.Image,
                CategoryId = category.CategoryId,
                ColorId = color.ColorId,
                ModelId = model.ModelId,
                UserId = user.UserId,
                SellStartDate = DateTime.UtcNow,
                SellEndDate = DateTime.UtcNow.AddMinutes(4), //AddDays()
            };

            var res = await _prodrepo.AddAsync(prod);
            await _context.SaveChangesAsync();
            return res;
        }

        public async Task<Product> UpdatePrice(PriceDTO price)
        {
            var product = await _prodrepo.GetByIdAsync(price.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found!");
            }
            if (product.UserId != _currentUser.Id)
            {
                throw new Exception("You are not the owner of this product!");
            }
            if (product.IsSaleEnded)
            {
                throw new Exception("Cannot update price — sale has ended!");
            }
            product.Price = price.Price;
            var updated = await _prodrepo.Update(product.ProductId, product);
            await _context.SaveChangesAsync();
            return updated;
        }

        public async Task<bool> UpdateSold(int productID)
        {
            var product = await _prodrepo.GetByIdAsync(productID);
            if (product == null)
            {
                return false;
            }
            if (product.UserId != _currentUser.Id)
            {
                throw new Exception("You are not the owner of this product!");
            }

            product.IsSold = true;
            product.IsSaleEnded = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> ExtendSale(ExtendSaleDTO sale)
        {
            var product = await _prodrepo.GetByIdAsync(sale.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }
            if (product.UserId != _currentUser.Id)
            {
                throw new Exception("You are not the owner of this product!");
            }
            if (sale.NewEndDate <= DateTime.Now)
            {
                throw new Exception("New end date must be in the future.");
            }
            product.SellEndDate = sale.NewEndDate;
            product.IsSaleEnded = false;
            await _context.SaveChangesAsync();
            return await _prodrepo.Update(product.ProductId, product);
        }


        public async Task<Product> DeleteProduct(int productId, string productName)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst("MyApp_Id")?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            int currentUserId = int.Parse(userIdString);

            var product = await _prodrepo.GetByIdAsync(productId);
            if (product == null || product.ProductName.ToLower() != productName.ToLower())
            {
                throw new Exception("Product not found!");
            }

            if (product.UserId != currentUserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this product.");
            }
            var result = await _prodrepo.Delete(productId);
            await _context.SaveChangesAsync();
            return result;
        }

        // public async Task<IEnumerable<Product>> GetAllAsync()
        // {
        //     return await _prodrepo.GetAllAsync();
        // }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = await _prodrepo.GetAllAsync();
            // var now = DateTime.Now;

            // foreach (var prod in products)
            // {
            //     // If end date has passed and not marked yet
            //     if (prod.SellEndDate.HasValue && prod.SellEndDate.Value < now)
            //     {
            //         if (prod.IsSaleEnded == false)
            //         {
            //             prod.IsSaleEnded = true;
            //             await _prodrepo.Update(prod.ProductId, prod);
            //         } 
            //     }
            // }

            // Return only active (non-ended) products
            return await _context.Products.Where(p => p.IsSaleEnded == false && p.IsSold == false).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync() //including both sold-unsold
        {
            return await _prodrepo.GetAllAsync();
        }

        public async Task<IEnumerable<Product>> MyProducts()
        {
            var products = await _prodrepo.GetAllAsync();
            var res = products.Where(p => p.UserId == _currentUser.Id).ToList();
            return res;
        }

        public async Task<IEnumerable<Product>> GetFilteredProducts(string cat, string color, string model, string prodname)
        {
            var products = await _prodrepo.GetAllAsync();

            if (!string.IsNullOrEmpty(cat))
            {
                var categ = await _otherService.GetCategoryByName(cat);
                products = products
                    .Where(p => p.Category != null &&
                                p.Category.Name.ToLower().Contains(cat.ToLower()) &&
                                p.CategoryId == categ.CategoryId);
            }

            if (!string.IsNullOrEmpty(color))
            {
                var col = await _otherService.GetColorByName(color);
                products = products
                    .Where(p => p.Color != null &&
                                p.Color.ColorName.ToLower().Contains(color.ToLower()) &&
                                p.ColorId == col.ColorId);
            }

            if (!string.IsNullOrEmpty(model))
            {
                var mod = await _otherService.GetModelByName(model);
                products = products
                    .Where(p => p.Model != null &&
                                p.Model.ModelName.ToLower().Contains(model.ToLower()) &&
                                p.ModelId == mod.ModelId);
            }

            if (!string.IsNullOrEmpty(prodname))
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(prodname.ToLower()));
            }

            products = products.Where(p => p.IsSold == false);

            return products;
        }

    }
}