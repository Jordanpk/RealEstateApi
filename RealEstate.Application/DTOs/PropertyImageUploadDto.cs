

using Microsoft.AspNetCore.Http;

namespace RealEstate.Application.DTOs
{
    public class PropertyImageUploadDto
    {
        public IFormFile File { get; set; } = default!;
    }
}
