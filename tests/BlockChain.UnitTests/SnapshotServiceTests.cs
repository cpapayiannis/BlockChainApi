using BlockChain.Application.Interfaces;
using BlockChain.Application.Services;
using BlockChain.Domain.Entities;
using BlockChain.Domain.Enums;
using FluentAssertions;
using Moq;

public class SnapshotServiceTests
{
    [Fact]
    public async Task FetchAndStoreAsync_Should_CallClient_And_SaveSnapshot()
    {
        var repo = new Mock<ISnapshotRepository>();
        var client = new Mock<IBlockCypherClient>();

        client.Setup(x => x.GetBlockchainDataAsync(BlockchainType.BTC_MAIN, It.IsAny<CancellationToken>()))
              .ReturnsAsync("{\"ok\":true}");

        repo.Setup(x => x.AddSnapshot(It.IsAny<BlockchainSnapshot>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Guid.NewGuid());

        var sut = new SnapshotService(repo.Object, client.Object);

        var id = await sut.FetchAndStoreAsync(BlockchainType.BTC_MAIN, CancellationToken.None);

        id.Should().NotBe(Guid.Empty);
        client.Verify(x => x.GetBlockchainDataAsync(BlockchainType.BTC_MAIN, It.IsAny<CancellationToken>()), Times.Once);
        repo.Verify(x => x.AddSnapshot(It.IsAny<BlockchainSnapshot>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
