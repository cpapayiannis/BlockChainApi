using BlockChain.Application.Interfaces;
using BlockChain.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlockChain.Infrastructure.Classes
{
    public class SnapshotRepository : ISnapshotRepository
    {
        private readonly AppDbContext _db;

        public SnapshotRepository(AppDbContext db) => _db = db;

        public Guid AddSnapshot(BlockchainSnapshot snapshot)
        {
            _db.BlockchainSnapshots.Add(snapshot);
            return snapshot.Id;

        }

        public async Task<IReadOnlyList<BlockchainSnapshot>> GetHistoryAsync(
            string chain,
            int page,
            int pageSize,
            CancellationToken ct)
        {
            var skip = (page - 1) * pageSize;

            return await _db.BlockchainSnapshots
                .AsNoTracking()
                .Where(x => x.Chain == chain)
                .OrderByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        public async Task<BlockchainSnapshot?> GetLatestAsync(string chain, CancellationToken ct)
        {
            return await _db.BlockchainSnapshots
                .AsNoTracking()
                .Where(x => x.Chain == chain)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(ct);
        }
    }
}