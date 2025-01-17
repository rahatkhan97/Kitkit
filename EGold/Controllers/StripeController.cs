using EGold.Resources;
using EGold.Services;
using Microsoft.AspNetCore.Mvc;

namespace EGold.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StripeController : ControllerBase
{
    private readonly IStripeService _stripeService;

    public StripeController(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("customer")]
    public async Task<ActionResult<CustomerResource>> CreateCustomer([FromBody] CreateCustomerResource resource,
        CancellationToken cancellationToken)
    {
        var response = await _stripeService.CreateCustomer(resource, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("charge")]
    public async Task<ActionResult<ChargeResource>> CreateCharge([FromBody] CreateChargeResource resource, CancellationToken cancellationToken)
    {
        var response = await _stripeService.CreateCharge(resource, cancellationToken);
        return Ok(response);
    }
    [HttpGet("charge-history/{customerId}")]
    public async Task<IActionResult> GetChargeHistory(string customerId, CancellationToken cancellationToken)
    {
        var charges = await _stripeService.GetChargeHistory(customerId, cancellationToken);
        if (!charges.Any())
        {
            return NotFound("No charges found for the specified customer.");
        }

        return Ok(charges);
    }
}