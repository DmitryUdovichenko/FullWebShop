namespace Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastMidofiedBy { get; set; }
        public DateTime? LastMidofiedDate { get; set; }
    }
}
