using BlockChain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Application.Interfaces
{
    public interface ISnapshotRepository
    {
        Guid AddSnapshot(BlockchainSnapshot snapshot, CancellationToken ct);

        Task<IReadOnlyList<BlockchainSnapshot>> GetHistoryAsync(
            string chain,
            int page,
            int pageSize,
            CancellationToken ct);

        Task<BlockchainSnapshot?> GetLatestAsync(string chain, CancellationToken ct);
    }
}
