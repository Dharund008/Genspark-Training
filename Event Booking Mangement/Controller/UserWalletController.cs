using System.Security.Claims;
using System.Threading.Tasks;
using EventBookingApi.Context;
using EventBookingApi.Interface;
using EventBookingApi.Model.DTO;
using EventBookingApi.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventBookingApi.Controller
{
    [ApiController]
    [Route("api/v1/wallet")]
    public class WalletController : ControllerBase
    {
    private readonly IUserWalletService _walletService;
    private readonly IOtherFunctionalities _other;

    public WalletController(IUserWalletService walletService, IOtherFunctionalities other)
    {
        _walletService = walletService;
        _other = other;
    }

    [HttpGet]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetMyWallet()
    {
        var userId = _other.GetLoggedInUserId(User);
        var wallet = await _walletService.GetWalletByUserId(userId);

        if (wallet == null)
            return NotFound(ApiResponse<object>.ErrorResponse("Wallet not found", null));

        var response = new
        {
            Balance = wallet.WalletBalance,
            Expiry = wallet.WalletBalanceExpiry?.ToString()  ?? "No Expiry",
            IsExpired = _walletService.IsWalletExpired(wallet)
        };

        return Ok(response);
    }
}
}
