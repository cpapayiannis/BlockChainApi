using BlockChain.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlockChain.Infrastructure.Classes;

public class AppDbContext : DbContext
{
    public DbSet<BlockchainSnapshot> BlockchainSnapshots => Set<BlockchainSnapshot>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var e = modelBuilder.Entity<BlockchainSnapshot>();

        e.ToTable("BlockchainSnapshots");
        e.HasKey(x => x.Id);

        e.Property(x => x.Chain).IsRequired().HasMaxLength(32);
        e.Property(x => x.Json).IsRequired();
        e.Property(x => x.CreatedAt).IsRequired();

        e.HasIndex(x => new { x.Chain, x.CreatedAt });
    }
}