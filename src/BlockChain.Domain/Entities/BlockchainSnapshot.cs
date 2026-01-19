using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Domain.Entities
{
    public class BlockchainSnapshot
    {
        public Guid Id { get; set; }
        public string Chain { get; set; } = null!;
        public string Json { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
