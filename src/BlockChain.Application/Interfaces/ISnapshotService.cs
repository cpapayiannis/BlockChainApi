using BlockChain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Application.Interfaces
{
    public interface ISnapshotService
    {
        Task<Guid> FetchAndStoreAsync(BlockchainType chain, CancellationToken ct);
        Task<IReadOnlyList<Guid>> FetchAndStoreAllAsync(CancellationToken ct);
    }
}
