using BlockChain.Application.Interfaces;
using BlockChain.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Application.Classes
{
    public class BlockCypherClient : IBlockCypherClient
    {
        private readonly HttpClient _http;

        private static readonly Dictionary<BlockchainType, string> Urls = new()
    {
        { BlockchainType.ETH_MAIN, "https://api.blockcypher.com/v1/eth/main" },
        { BlockchainType.DASH_MAIN, "https://api.blockcypher.com/v1/dash/main" },
        { BlockchainType.BTC_MAIN, "https://api.blockcypher.com/v1/btc/main" },
        { BlockchainType.BTC_TEST3, "https://api.blockcypher.com/v1/btc/test3" },
        { BlockchainType.LTC_MAIN, "https://api.blockcypher.com/v1/ltc/main" }
    };

        public BlockCypherClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<string> GetBlockchainDataAsync(BlockchainType chain, CancellationToken ct)
        {
            var response = await _http.GetAsync(Urls[chain], ct);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(ct);
        }
    }
}
