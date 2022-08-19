using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Identity
{
    public class Address : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; }        
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}