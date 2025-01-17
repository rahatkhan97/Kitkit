using EGold.Models;
using EGold.Resources;
using EGold.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EGold.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService userService)
        {
        _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateDto model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var user = await _userService.RegisterAsync(registerDto);
            if (user == null)
                return BadRequest("Registration failed.");

            return Ok(new { Token = user.Token });
        }
        [Authorize]
        [HttpPost("add-card")]
        public async Task<IActionResult> AddCard(int userId, [FromBody] CreateCustomerResource resource, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userService.AddCardAsync(userId, resource, cancellationToken);
                return Ok(new { Message = "Card added successfully", StripeCustomerId = user.StripeCustomerId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        //[Authorize]
        //[HttpGet]
        //    public IActionResult GetAll()
        //    {
        //        var users = _userService.GetAll();
        //        return Ok(users);
        //    }
    }
}
