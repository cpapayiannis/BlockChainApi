using BlockChain.Application.DTOs;
using BlockChain.Domain.Entities;

namespace BlockChain.Application.Mapping
{
    public static class SnapshotMapping
    {
        public static SnapshotDto ToDto(this BlockchainSnapshot e) => new()
        {
            Id = e.Id,
            Chain = e.Chain,
            Json = e.Json,
            CreatedAt = e.CreatedAt
        };
    }
}
