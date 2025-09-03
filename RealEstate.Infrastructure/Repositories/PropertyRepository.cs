using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Repositories;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly RealEstateDbContext _db;
        public PropertyRepository(RealEstateDbContext db) => _db = db;

        public async Task<Property> AddAsync(Property entity, CancellationToken ct)
        {
            await _db.Properties.AddAsync(entity, ct);
            return entity;
        }

        public async Task<Property?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _db.Properties
                .Include(p => p.Images)
                .Include(p => p.Traces)
                .Include(p => p.Owner)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }


        public Task UpdateAsync(Property entity, CancellationToken ct)
        {
            _db.Properties.Update(entity);
            return Task.CompletedTask;
        }

        public async Task AddImageAsync(PropertyImage image, CancellationToken ct)
        {
            await _db.PropertyImages.AddAsync(image, ct);
        }

        public async Task AddTraceAsync(PropertyTrace trace, CancellationToken ct)
        {
            await _db.PropertyTraces.AddAsync(trace, ct);
        }

        public async Task ChangePriceAsync(int propertyId, decimal newPrice, CancellationToken ct)
        {
            var p = await _db.Properties.FirstOrDefaultAsync(x => x.Id == propertyId, ct)
                    ?? throw new KeyNotFoundException("Propiedad no encontrada");
            p.ChangePrice(newPrice);
        }

        public async Task<(IReadOnlyList<Property> Items, int Total)> ListAsync(
           int? propertyId, string? city, string? state, decimal? minPrice, decimal? maxPrice,
           int? Year, CancellationToken ct)
        {
            var q = _db.Properties
                       .AsNoTracking()
                       .Include(p => p.Images)
                       .Include(p => p.Owner)
                       .AsQueryable();

            if (propertyId.HasValue) q = q.Where(p => p.Id == propertyId.Value);
            if (!string.IsNullOrWhiteSpace(city)) q = q.Where(p => p.Address.Contains(city));
            if (!string.IsNullOrWhiteSpace(state)) q = q.Where(p => p.Address.Contains(state));
            if (minPrice.HasValue) q = q.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue) q = q.Where(p => p.Price <= maxPrice.Value);
            if (Year.HasValue) q = q.Where(p => p.Year == Year.Value);

            var total = await q.CountAsync(ct);
            var items = await q.OrderBy(p => p.Price).ToListAsync(ct);

            return (items, total);
        }

    }
}
