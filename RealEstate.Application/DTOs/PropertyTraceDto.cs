using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.DTOs
{
    public class PropertyTraceDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha de venta es obligatoria")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha inválido. Ejemplo: 2025-09-02")]
        public DateTime DateSale { get; set; }

        [Required(ErrorMessage = "El valor es obligatorio")]
        [Range(1, double.MaxValue, ErrorMessage = "El valor debe ser mayor a 0")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "El nombre del comprador es obligatorio")]
        [StringLength(150)]
        public string BuyerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe especificar la propiedad")]
        public int PropertyId { get; set; }
    }
}
