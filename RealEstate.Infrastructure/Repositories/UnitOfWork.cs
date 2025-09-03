using System.Threading;
using System.Threading.Tasks;
using RealEstate.Domain.Repositories;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RealEstateDbContext _db;
        public UnitOfWork(RealEstateDbContext db) => _db = db;
        public Task<int> SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
