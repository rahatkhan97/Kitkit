using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EGold.Models
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string UserName { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [JsonIgnore]
        public string Password { get; set; }
        public string? Token { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        // Stripe Customer ID
        [Column(TypeName = "nvarchar(100)")]
        public string? StripeCustomerId { get; set; }

        // Navigation property to track user token balances
        public ICollection<UserTokenEntity> UserTokens { get; set; } = new List<UserTokenEntity>();
    }
}
