using System.ComponentModel.DataAnnotations;

namespace Shop.API.Dtos
{
    public class RegistrationDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?!.*\\s).{4,8}$", ErrorMessage = "To weak password")]
        public string Password { get; set; }
    }
}