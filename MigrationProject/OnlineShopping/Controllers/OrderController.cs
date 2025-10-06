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
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("placing-order")]
        public async Task<IActionResult> PlaceOrder(IEnumerable<int> productids)
        {
            try
            {
                if (productids != null)
                {
                    var ordered = await _orderService.PlaceOrder(productids);
                    return Ok(new { message = "Order placed successfully!", ordered });
                }
                return Ok(new { message = "Product ID list is empty or null!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OrderController : PlaceOrder, please try again later.", ex.Message });
            }
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyorders()
        {
            try
            {
                var ordered = await _orderService.GetMyOrders();
                if (ordered != null && ordered.Any())
                {
                    return Ok(new { message = "Orders retrieved successfully!", ordered });
                }
                return Ok(new { message = "No orders found for this user!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OrderController : GetMyorders, please try again later.", ex.Message });
            }
        }
    }

}