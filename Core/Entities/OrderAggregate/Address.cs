namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public Address()
        {
        }

        public Address(string firstName, string lastName, string city, string postCode, string street)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            PostCode = postCode;
            Street = street;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Street { get; set; } 
    }
}