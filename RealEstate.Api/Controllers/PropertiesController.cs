using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;

namespace RealEstate.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertyService _service;
        private readonly IWebHostEnvironment _env;

        public PropertiesController(IPropertyService service, IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        /// <summary>
        /// List property with filters
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] PropertyFilterDto filter, CancellationToken ct)
        {
            var (items, total) = await _service.ListAsync(filter, ct);

            foreach (var prop in items)
            {
                foreach (var img in prop.Images)
                {
                    img.Url = $"{Request.Scheme}://{Request.Host}/images/{img.File}";
                }
            }

            return Ok(new { total, items });
        }

        /// <summary>
        /// Create Property Building
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PropertyDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] PropertyCreateDto dto, CancellationToken ct)
        {
            var result = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(List), new { id = result.Id }, result);
        }

        /// <summary>
        /// Add Image from property
        /// </summary>
        [HttpPost("{id:int}/images")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UploadImage(int id, [FromForm] PropertyImageUploadDto uploadDto, CancellationToken ct)
        {
            if (uploadDto.File == null || uploadDto.File.Length == 0)
                return BadRequest("Debe seleccionar un archivo de imagen.");

            var file = uploadDto.File;

            // Validar extensión
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            if (!allowedExtensions.Contains(extension))
                return BadRequest("Formato no válido. Solo se permiten .jpg y .png");

            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var fullFileName = $"{fileName}{extension}";

            // Validar propiedad existente
            var property = await _service.GetByIdAsync(id, ct);
            if (property == null)
                return NotFound($"No existe la propiedad con Id={id}");

            // Validar duplicado
            if (property.Images.Any(img => string.Equals(img.File, fullFileName, StringComparison.OrdinalIgnoreCase)))
                return Conflict($"La propiedad {id} ya tiene una imagen llamada {fullFileName}");

            var projectRoot = Directory.GetCurrentDirectory();
            var imagesPath = Path.Combine(projectRoot, "images");
            if (!Directory.Exists(imagesPath))
                Directory.CreateDirectory(imagesPath);

            var filePath = Path.Combine(imagesPath, fullFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream, ct);
            }

            var dto = new PropertyImageCreateDto
            {
                File = fullFileName,
                Enabled = true
            };

            await _service.AddImageAsync(id, dto, ct);

            return Ok(new { message = "Imagen cargada correctamente", fileName = fullFileName });
        }

        /// <summary>
        /// Change Price
        /// </summary>
        [HttpPut("{id:int}/price")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePrice(int id, [FromBody] decimal newPrice, CancellationToken ct)
        {
            await _service.ChangePriceAsync(id, newPrice, ct);
            return NoContent();
        }

        /// <summary>
        /// Update property
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] PropertyUpdateDto dto, CancellationToken ct)
        {
            await _service.UpdateAsync(id, dto, ct);
            return NoContent();
        }
    }
}
