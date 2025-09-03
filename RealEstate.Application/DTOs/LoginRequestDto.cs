using System.ComponentModel.DataAnnotations;

namespace RealEstate.Application.DTOs
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La clave es obligatoria")]
        public string Password { get; set; } = string.Empty;
    }
}
