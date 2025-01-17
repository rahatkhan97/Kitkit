using EGold.Models;
using EGold.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Threading;

namespace EGold.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenAppService _tokenService;
        public TokenController(ITokenAppService tokenService)
        {
            _tokenService = tokenService;
        }
        [Authorize]
        [HttpPost("buy-token")]
        public async Task<IActionResult> BuyTokenAsync([FromBody]BuyTokenDto buyToken)
        {
            try
            {
                var userToken = await _tokenService.BuyTokenAsync(buyToken.userId, buyToken.tokenId, buyToken.quantity, buyToken.price);
                return Ok(new { Message = "Token purchased successfully", UserToken = userToken });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
