namespace RealEstate.Application.DTOs
{
    public class PropertyDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? Year { get; set; }
        public string OwnerName { get; set; } = string.Empty;

        // Relación con imágenes
        public List<PropertyImageDto> Images { get; set; } = new();

        // Relación con trazas
        public List<PropertyTraceDto> Traces { get; set; } = new();
    }
}
