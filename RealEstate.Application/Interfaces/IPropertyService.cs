using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RealEstate.Application.DTOs;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<PropertyDto> CreateAsync(PropertyCreateDto dto, CancellationToken ct);
        Task AddImageAsync(int propertyId, PropertyImageCreateDto dto, CancellationToken ct);
        Task<PropertyDto?> GetByIdAsync(int id, CancellationToken ct);
        Task ChangePriceAsync(int propertyId, decimal newPrice, CancellationToken ct);
        Task UpdateAsync(int propertyId, PropertyUpdateDto dto, CancellationToken ct);
        Task<(IReadOnlyList<PropertyDto> Items, int Total)> ListAsync(PropertyFilterDto filter, CancellationToken ct);
    }
}
