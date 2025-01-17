using EGold.Helpers;
using EGold.Models;
using EGold.Resources;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EGold.Services
{
    public interface IUserService
    {
        UserEntity Authenticate(string Email, string Password);
        Task<UserEntity> RegisterAsync(RegisterDto registerDto);
        IEnumerable<UserEntity> GetAll();
        Task<UserEntity> AddCardAsync(int userId, CreateCustomerResource resource, CancellationToken cancellationToken);
    }
    public class UserService : IUserService
    {
        private readonly EGoldDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly ILogger<UserService> _logger;
        private readonly IStripeService _stripeService;

        public UserService(EGoldDbContext context, IOptions<AppSettings> appSettings, ILogger<UserService> logger, IStripeService stripeService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _logger = logger;
            _stripeService = stripeService;
        }

        public UserEntity Authenticate(string Email, string Password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == Email);

            // return null if user not found
            if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password))
                return null;

            //// authentication successful so generate jwt token
            user.Token = GenerateJwtToken(user);

            // Return essential data
            user.Password = null; // Don't send the password in the response
            return user;
        }
        private string GenerateJwtToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expires after 7 days
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<UserEntity> RegisterAsync(RegisterDto registerDto)
        {
            var user = new UserEntity
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = $"{registerDto.FirstName}{registerDto.LastName}",
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password), // Hash the password
                PhoneNumber = registerDto.PhoneNumber
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Now generate a token after registration
            user.Token = GenerateJwtToken(user);

            // Save the user with the token
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<UserEntity> AddCardAsync(int userId, CreateCustomerResource resource, CancellationToken cancellationToken)
        {
            // Retrieve the user from the database
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            // Call the StripeService method to add a card and create a Stripe customer if needed
            var customer = await _stripeService.CreateCustomer(resource,cancellationToken);
            if (customer is null)
                throw new Exception("Stripe customer not found.");

            if (string.IsNullOrEmpty(customer.CustomerId))
            {
                user.StripeCustomerId = customer.CustomerId;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }
        public IEnumerable<UserEntity> GetAll()
        {
            return _context.Users;
        }
    }
}
