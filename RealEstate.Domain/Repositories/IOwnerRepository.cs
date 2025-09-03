public interface IOwnerRepository
{
    Task<bool> ExistsAsync(int ownerId, CancellationToken ct);
}