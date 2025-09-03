using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Repositories;
using RealEstate.Infrastructure.Persistence;

public class OwnerRepository : IOwnerRepository
{
    private readonly RealEstateDbContext _db;

    public OwnerRepository(RealEstateDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExistsAsync(int ownerId, CancellationToken ct)
    {
        return _db.Owners.AnyAsync(o => o.Id == ownerId, ct);
    }
}
