using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.DTOs
{
    public class PropertyImageCreateDto
    {
        [Required(ErrorMessage = "El nombre del archivo es obligatorio")]
        [StringLength(200, ErrorMessage = "El nombre del archivo no puede exceder 200 caracteres")]
        public string File { get; set; } = string.Empty;

        public bool Enabled { get; set; } = true;
    }
}
