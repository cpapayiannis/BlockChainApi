using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockChain.Domain.Enums;

namespace BlockChain.Application.Interfaces;

public interface IBlockCypherClient
{
    Task<string> GetBlockchainDataAsync(BlockchainType chain, CancellationToken ct);
}
