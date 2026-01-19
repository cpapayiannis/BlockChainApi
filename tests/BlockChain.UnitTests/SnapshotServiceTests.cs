using BlockChain.Application.Interfaces;
using BlockChain.Application.Services;
using BlockChain.Domain.Entities;
using BlockChain.Domain.Enums;
using FluentAssertions;
using Moq;

public class SnapshotServiceTests
{
    [Fact]
    public async Task FetchAndStoreAsync_ShouldFetchAndPersist()
    {
        var repo = new Mock<ISnapshotRepository>();
        var client = new Mock<IBlockCypherClient>();
        var uow = new Mock<IUnitOfWork>();

        client.Setup(c => c.GetBlockchainDataAsync(BlockchainType.BTC_MAIN, It.IsAny<CancellationToken>()))
              .ReturnsAsync("{json}");

        // Repository no longer saves; it just adds
        repo.Setup(r => r.AddSnapshot(It.IsAny<BlockchainSnapshot>()));

        // UnitOfWork is responsible for SaveChanges
        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
           .ReturnsAsync(1);

        var sut = new SnapshotService(repo.Object, client.Object, uow.Object);

        var id = await sut.FetchAndStoreAsync(BlockchainType.BTC_MAIN, CancellationToken.None);

        id.Should().NotBe(Guid.Empty);

        client.Verify(x => x.GetBlockchainDataAsync(BlockchainType.BTC_MAIN, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(x => x.AddSnapshot(It.IsAny<BlockchainSnapshot>()), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
