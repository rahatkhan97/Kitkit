namespace EGold.Models
{
    public class BuyTokenDto
    {
        public int userId { get; set; }
        public int tokenId { get; set; }
        public decimal quantity { get; set; }
        public decimal price { get; set; }
    }
}
