using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.DTOs
{
    public class PropertyCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(250, ErrorMessage = "La dirección no puede tener más de 250 caracteres")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser un número mayor a 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "El código interno es obligatorio")]
        [StringLength(50, ErrorMessage = "El código interno no puede tener más de 50 caracteres")]
        public string CodeInternal { get; set; } = string.Empty;

        [Range(1800, 2100, ErrorMessage = "El año debe estar entre 1800 y 2100")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Debe ingresar un propietario válido")]
        public int OwnerId { get; set; }    // FK al owner
    }
}
