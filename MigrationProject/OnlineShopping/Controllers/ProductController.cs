using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Online.Models;
using Online.Models.DTO;
using Online.Interfaces;
using Online.Contexts;
using Online.Services;

namespace Online.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("upload-product-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                //_logger.LogWarning("File not selected in UploadScreenshot");
                return BadRequest("File not selected");
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var savePath = Path.Combine("wwwroot/Products", fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            //_logger.LogInformation("Uploaded screenshot {FileName}", fileName);
            return Ok(new { url = $"/ProductImage/{fileName}" });
        }


        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] AddprdDTO req)
        {
            try
            {
                if (req != null)
                {
                    var result = await _productService.AddProduct(req);
                    return Ok(new { message = "Product added successfully!", result });
                }
                return BadRequest(new { message = "Failed to add product!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : AddProduct, please try again later.", ex.Message });
            }
        }

        

        [HttpPut("update-price")]
        public async Task<IActionResult> UpdatePrice([FromBody] PriceDTO req)
        {
            try
            {
                if (req != null)
                {
                    var result = await _productService.UpdatePrice(req);
                    return Ok(new { message = "Price updated successfully!", result });
                }
                return BadRequest(new { message = "Failed to update price!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : UpdatePrice, please try again later.", ex.Message });
            }
        }

        [HttpPut("update-used")]
        public async Task<IActionResult> UpdateSold([FromQuery] int productID)
        {
            try
            {
                if (productID != 0)
                {
                    var result = await _productService.UpdateSold(productID);
                    return Ok(new { message = "Product status-used updated successfully!", result });
                }
                return BadRequest(new { message = "Failed to update product status-used!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : UpdateUsed, please try again later.", ex.Message });
            }
        }

        [HttpPut("extend-sale")]
        public async Task<IActionResult> ExtendSale([FromBody] ExtendSaleDTO req)
        {
            try
            {
                if (req != null)
                {
                    var result = await _productService.ExtendSale(req);
                    return Ok(new { message = "Sale extended successfully!", result });
                }
                return BadRequest(new { message = "Failed to update extend sale!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : ExtendSale, please try again later.", ex.Message });
            }
        }

        [HttpDelete("delete-product")]
        public async Task<IActionResult> DeleteProduct([FromQuery] int productID, [FromQuery] string productName)
        {
            try
            {
                if (productID != 0 && productName != null)
                {
                    var result = await _productService.DeleteProduct(productID, productName);
                    return Ok(new { message = "Product deleted successfully!", result });
                }
                return BadRequest(new { message = "Failed to delete product!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : DeleteProduct, please try again later.", ex.Message });
            }
        }

        [HttpGet("unsold-product-by-name")]
        public async Task<IActionResult> GetProductByName([FromQuery] string productName)
        {
            try
            {
                if (productName != null)
                {
                    var result = await _productService.GetProductByName(productName.ToLower());
                    return Ok(new { message = "Product found successfully!", result });
                }
                return BadRequest(new { message = "Failed to find product!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : GetProductByName, please try again later.", ex.Message });
            }
        }

        [HttpGet("unsold-product-by-id")]
        public async Task<IActionResult> GetProductById([FromQuery] int productID)
        {
            try
            {
                if (productID != 0)
                {
                    var result = await _productService.GetByIdAsync(productID);
                    return Ok(new { message = "Product found successfully!", result });
                }
                return BadRequest(new { message = "Failed to find product!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : GetProductById, please try again later.", ex.Message });
            }
        }

        [HttpGet("unsold-get-all-product")]
        public async Task<IActionResult> GetAllUnSoldProduct()
        {
            try
            {
                var result = await _productService.GetAllAsync();
                if (result != null)
                {
                    return Ok(new { message = "Products found successfully!", result });
                }
                return BadRequest(new { message = "No products found!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : GetAllProduct, please try again later.", ex.Message });
            }
        }

        [HttpGet("unsold-filtered-products")]
        public async Task<IActionResult> GetFilteredProducts([FromQuery] string productName, [FromQuery] string productCategory, [FromQuery] string productModel, [FromQuery] string productColor)
        {
            try
            {
                if (productName != null || productCategory != null || productModel != null || productColor != null)
                {
                    var result = await _productService.GetFilteredProducts(productCategory.ToLower(), productColor.ToLower(), productModel.ToLower(), productName.ToLower());
                    if (result != null)
                    {
                        return Ok(new { message = "Filtered products found successfully!", result });
                    }
                }
                return BadRequest(new { message = "Failed to find filtered products!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : GetFilteredProducts, please try again later.", ex.Message });
            }
        }

        [HttpGet("sold-product-by-name")]
        public async Task<IActionResult> GetSoldProductByName([FromQuery] string productName)
        {
            try
            {
                if (productName != null)
                {
                    var result = await _productService.GetSoldProductByName(productName.ToLower());
                    if (result != null)
                    {
                        return Ok(new { message = "Sold product found successfully!", result });
                    }
                }
                return BadRequest(new { message = "Failed to find sold product!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : GetSoldProductByName, please try again later.", ex.Message });
            }
        }

        [HttpGet("all-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var result = await _productService.GetAllProductAsync();
                if (result != null)
                {
                    return Ok(new { message = "All products found successfully!", result });
                }
                return BadRequest(new { message = "Failed to find all products!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in ProductController : GetAllProducts, please try again later.", ex.Message });
            }
        }
       

    }
}