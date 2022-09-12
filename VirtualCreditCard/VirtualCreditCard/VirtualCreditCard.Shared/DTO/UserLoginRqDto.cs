using System.ComponentModel.DataAnnotations;

namespace VirtualCreditCard.Models.Dto
{
    public class UserLoginRqDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
