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

    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderDetailController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("my-order-details")]
        public async Task<IActionResult> GetMyOrderDetails(int orderid)
        {
            try
            {
                if (orderid <= 0)
                {
                    return Ok(new { message = "Invalid order ID provided!" });
                }

                var dts = await _orderService.GetMyOrderDetails(orderid);

                if (dts == null)
                {
                    return NotFound(new { message = "Order details not found for the given ID!" });
                }

                return Ok(new { message = "Order details retrieved successfully!", dts });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred in OrderDetailController : GetMyOrderDetails, please try again later.", ex.Message });
            }
        }
    }


}