using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Blockchain
{
    class BlockchainContext : DbContext
    {
        public BlockchainContext()
            : base("BlockchainConnection")
        { }

        public DbSet<Block> Blocks { get; set; }


    }
}
