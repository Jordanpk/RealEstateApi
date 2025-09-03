using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.DTOs
{
    public class PropertyUpdateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(250)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser un número mayor a 0")]
        public decimal Price { get; set; }

        [Range(1800, 2100, ErrorMessage = "El año debe estar entre 1800 y 2100")]
        public int? Year { get; set; }
    }
}
