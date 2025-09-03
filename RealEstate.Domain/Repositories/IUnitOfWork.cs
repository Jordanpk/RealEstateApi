using System.Threading;
using System.Threading.Tasks;

namespace RealEstate.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
