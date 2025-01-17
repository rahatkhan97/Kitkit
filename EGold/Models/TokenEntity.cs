using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EGold.Models
{
    public class TokenEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; } // Token name (e.g., Ethereum, Bitcoin)

        [Required]
        [Column(TypeName = "nvarchar(10)")]
        public string Symbol { get; set; } // Token symbol (e.g., ETH, BTC)

        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal TotalSupply { get; set; } // Total supply of the token

        //[Required]
        //[Column(TypeName = "decimal(18,8)")]
        //public decimal CirculatingSupply { get; set; } // Circulating supply of the token

        //[Required]
        //[Column(TypeName = "nvarchar(100)")]
        //public string ContractAddress { get; set; } // Contract address for blockchain tokens

        //[Required]
        //[Column(TypeName = "nvarchar(20)")]
        //public string Blockchain { get; set; } // Blockchain the token operates on (e.g., Ethereum, Binance Smart Chain)

        //[Column(TypeName = "decimal(18,8)")]
        //public decimal? MarketCap { get; set; } // Market cap of the token (optional, can be calculated)

        [Column(TypeName = "decimal(18,8)")]
        public decimal? CurrentPrice { get; set; } // Current price of the token in USD or base currency

        //[Column(TypeName = "decimal(18,8)")]
        //public decimal? Volume24h { get; set; } // 24-hour trading volume

        //[Column(TypeName = "decimal(18,8)")]
        //public decimal? PriceChangePercentage24h { get; set; } // Price change percentage in the last 24 hours

        [Column(TypeName = "nvarchar(500)")]
        public string Description { get; set; } // Brief description of the token

        //[Column(TypeName = "nvarchar(2083)")]
        //public string Website { get; set; } // Official website of the token

        //[Column(TypeName = "nvarchar(2083)")]
        //public string Whitepaper { get; set; } // Link to the token's whitepaper

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Creation timestamp
        public DateTime? UpdatedAt { get; set; } // Last update timestamp
    }
}
