using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EGold.Models
{
    public class UserTokenEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign key to UserEntity

        [Required]
        public int TokenId { get; set; } // Foreign key to TokenEntity

        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal Quantity { get; set; } // Amount of the token the user owns

        [Required]
        [Column(TypeName = "decimal(18,8)")]
        public decimal AveragePrice { get; set; } // Average purchase price of the token

        // Navigation properties
        public UserEntity User { get; set; }
        public TokenEntity Token { get; set; }
    }
}
