using System.ComponentModel.DataAnnotations;

namespace Shop.API.Dtos
{
    public class AddressDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string City { get; set; }

        [Required, StringLength(5, MinimumLength = 5)]
        public string PostCode { get; set; }

        [Required]
        public string Street { get; set; } 
    }
}