using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Application.DTOs
{
    public sealed class SnapshotDto
    {
        public Guid Id { get; set; }
        public string Chain { get; set; } = default!;
        public string Json { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
