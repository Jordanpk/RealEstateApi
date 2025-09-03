using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Repositories
{
    public interface IPropertyRepository
    {
        Task<Property> AddAsync(Property entity, CancellationToken ct);
        Task<Property?> GetByIdAsync(int id, CancellationToken ct);
        Task UpdateAsync(Property entity, CancellationToken ct);
        Task AddImageAsync(PropertyImage image, CancellationToken ct);
        Task AddTraceAsync(PropertyTrace trace, CancellationToken ct);
        Task ChangePriceAsync(int propertyId, decimal newPrice, CancellationToken ct);
        Task<(IReadOnlyList<Property> Items, int Total)> ListAsync(
           int? propertyId, string? city, string? state, decimal? minPrice,
           decimal? maxPrice, int? Year, CancellationToken ct);

    }
}
