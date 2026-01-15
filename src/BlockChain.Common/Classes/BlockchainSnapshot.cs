using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Common.Classes
{
    public class BlockchainSnapshot
    {
        public int Id { get; set; }
        public string Chain { get; set; } = null!;
        public string Json { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
    }
}
