using EGold.Models;
using Microsoft.EntityFrameworkCore;

namespace EGold.Services
{
    public interface ITokenAppService
    {
        Task<UserTokenEntity> BuyTokenAsync(int userId, int tokenId, decimal quantity, decimal price);
    }

    public class TokenAppService : ITokenAppService
    {
        private readonly EGoldDbContext _context;
        public TokenAppService(EGoldDbContext context)
        {
            _context = context;
        }
        public async Task<UserTokenEntity> BuyTokenAsync(int userId, int tokenId, decimal quantity, decimal price)
        {
            var user = await _context.Users.Include(u => u.UserTokens).SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("User not found.");

            var token = await _context.Tokens.SingleOrDefaultAsync(t => t.Id == tokenId);
            if (token == null) throw new Exception("Token not found.");

            var userToken = user.UserTokens.FirstOrDefault(ut => ut.TokenId == tokenId);
            if (userToken == null)
            {
                userToken = new UserTokenEntity
                {
                    UserId = userId,
                    TokenId = tokenId,
                    Quantity = quantity,
                    AveragePrice = price
                };
                _context.UserTokens.Add(userToken);
            }
            else
            {
                // Update quantity and average price
                userToken.Quantity += quantity;
                userToken.AveragePrice = ((userToken.AveragePrice * userToken.Quantity) + (price * quantity)) / (userToken.Quantity + quantity);
                _context.UserTokens.Update(userToken);
            }

            await _context.SaveChangesAsync();
            return userToken;
        }
    }
}
