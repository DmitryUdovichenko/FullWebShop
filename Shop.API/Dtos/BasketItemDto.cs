using System.ComponentModel.DataAnnotations;

namespace Shop.API.Dtos
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price shouldn't be zerro")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Quantity shouldn't be zerro")]
        public int Quantity { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}