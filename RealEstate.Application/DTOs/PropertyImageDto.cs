namespace RealEstate.Application.DTOs
{
    public class PropertyImageDto
    {
        public int Id { get; set; }
        public string File { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public bool Enabled { get; set; }

        // Relación con propiedad
        public int PropertyId { get; set; }
    }
}
