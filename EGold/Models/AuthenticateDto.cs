using System.ComponentModel.DataAnnotations;

namespace EGold.Models
{
    public class AuthenticateDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
