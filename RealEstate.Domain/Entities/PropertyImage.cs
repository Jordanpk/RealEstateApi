namespace RealEstate.Domain.Entities
{
    public class PropertyImage
    {
        public int Id { get; set; }            // IdPropertyImage en DB
        public int PropertyId { get; set; }    // IdProperty
        public Property? Property { get; set; }
        public string File { get; set; } = null!;
        public bool Enabled { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
