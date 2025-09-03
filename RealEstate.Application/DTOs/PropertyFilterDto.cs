namespace RealEstate.Application.DTOs
{
    public class PropertyFilterDto
    {
        public int? PropertyId { get; set; }   
        public string? City { get; set; }
        public string? State { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Year { get; set; }
    }
}
