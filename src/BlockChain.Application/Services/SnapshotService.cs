using BlockChain.Application.Interfaces;
using BlockChain.Domain.Entities;
using BlockChain.Domain.Enums;

namespace BlockChain.Application.Services;

public class SnapshotService : ISnapshotService
{
    private readonly ISnapshotRepository _repository;
    private readonly IBlockCypherClient _client;
    private readonly IUnitOfWork _uow;

    public SnapshotService(ISnapshotRepository repository, IBlockCypherClient client, IUnitOfWork uow)
    {
        _repository = repository;
        _client = client;
        _uow = uow;
    }

    public async Task<Guid> FetchAndStoreAsync(BlockchainType chain, CancellationToken ct)
    {
        var json = await _client.GetBlockchainDataAsync(chain, ct);

        var snapshot = new BlockchainSnapshot
        {
            Id = Guid.NewGuid(),
            Chain = chain.ToString(),
            Json = json,
            CreatedAt = DateTime.UtcNow
        };

       _repository.AddSnapshot(snapshot);
        await _uow.SaveChangesAsync(ct);
        return snapshot.Id;

    }

    public async Task<IReadOnlyList<Guid>> FetchAndStoreAllAsync(CancellationToken ct)
    {
        var chains = Enum.GetValues<BlockchainType>();

        var fetchTasks = chains.Select(async chain => new
        {
            Chain = chain,
            Json = await _client.GetBlockchainDataAsync(chain, ct)
        });

        var results = await Task.WhenAll(fetchTasks);

        var ids = new List<Guid>();

        foreach (var r in results)
        {
            var snapshot = new BlockchainSnapshot
            {
                Id = Guid.NewGuid(),
                Chain = r.Chain.ToString(),
                Json = r.Json,
                CreatedAt = DateTime.UtcNow
            };

            _repository.AddSnapshot(snapshot);
            await _uow.SaveChangesAsync(ct);
            ids.Add(snapshot.Id);
        }

        return ids;
    }
}
