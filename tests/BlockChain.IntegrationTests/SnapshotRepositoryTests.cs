
using BlockChain.Domain.Entities;
using BlockChain.Infrastructure.Classes;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class SnapshotRepositoryTests
{
    [Fact]
    public async Task GetLatest_ShouldReturnMostRecentSnapshot()
    {
        var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(conn)
            .Options;

        using var ctx = new AppDbContext(options);
        ctx.Database.EnsureCreated();

        var repo = new SnapshotRepository(ctx);

        repo.AddSnapshot(new BlockchainSnapshot
        {
            Id = Guid.NewGuid(),
            Chain = "BTC_MAIN",
            Json = "old",
            CreatedAt = DateTime.UtcNow.AddMinutes(-10)
        }, CancellationToken.None);

        repo.AddSnapshot(new BlockchainSnapshot
        {
            Id = Guid.NewGuid(),
            Chain = "BTC_MAIN",
            Json = "new",
            CreatedAt = DateTime.UtcNow
        }, CancellationToken.None);

        var latest = await repo.GetLatestAsync("BTC_MAIN", CancellationToken.None);

        latest!.Json.Should().Be("new");
    }
}
