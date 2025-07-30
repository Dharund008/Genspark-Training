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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add-to-cart")]
        public async Task<IActionResult> Addtocart([FromQuery] int productid)
        {
            try
            {
                if (productid != 0)
                {
                    var added = await _cartService.AddtoCart(productid);
                    return Ok(new { message = "Product added to cart successfully!", added });
                }
                return BadRequest(new { message = "Invalid product ID!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in CartController : Addtocart, please try again later.", ex.Message });
            }
        }

        [HttpPost("buy-this-product-from-cart")]
        public async Task<IActionResult> BuythisCart([FromQuery] int productid)
        {
            try
            {
                if (productid != 0)
                {
                    var bought = await _cartService.BuySpecificItemFromCart(productid);
                    return Ok(new { message = "Product purchased successfully from cart!", bought });
                }
                return BadRequest(new { message = "Invalid product ID!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in CartController : BuythisCart, please try again later.", ex.Message });
            }
        }

        [HttpGet("my-cart")]
        public async Task<IActionResult> ViewMyCart()
        {
            try
            {
                var mycart = await _cartService.GetMyCart();
                if (mycart != null && mycart.Any())
                {
                    return Ok(new { message = "Cart retrieved successfully!", mycart });
                }
                return BadRequest(new { message = "Nothing in your cart!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in CartController : ViewMyCart, please try again later.", ex.Message });
            }
        }

        [HttpGet("dispatch-my-cart")]
        public async Task<IActionResult> BuyMyCart()
        {
            try
            {
                var ordered = await _cartService.BuyAllCart();
                return Ok(new { message = "Cart dispatched successfully!", ordered });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in CartController : BuyMyCart, please try again later.", ex.Message });
            }
        }

        [HttpDelete("remove-item-from-cart")]
        public async Task<IActionResult> Removefromcart([FromQuery] int productid)
        {
            try
            {
                if (productid != 0)
                {
                    var removed = await _cartService.RemoveItemFromCart(productid);
                    if (removed != null)
                    {
                        return Ok(new { message = "Item removed from cart successfully!", removed });
                    }
                    return BadRequest(new { message = "No such product exists in your cart!" });
                }
                return BadRequest(new { message = "Invalid product ID!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in CartController : Removefromcart, please try again later.", ex.Message });
            }
        }
    }

}