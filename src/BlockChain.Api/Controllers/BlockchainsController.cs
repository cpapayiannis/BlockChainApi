using BlockChain.Api.Models;
using BlockChain.Application.Interfaces;
using BlockChain.Application.Mapping;
using BlockChain.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlockChain.Api.Controllers;

[ApiController]
[Route("api/v1/blockchains")]
public class BlockchainsController : ControllerBase
{
    private readonly ISnapshotService _service;
    private readonly ISnapshotRepository _repository;
    private readonly ILogger<BlockchainsController> _logger;

    public BlockchainsController(ILogger<BlockchainsController> logger,ISnapshotService service, ISnapshotRepository repository)
    {
        _service = service;
        _repository = repository;
        _logger = logger;
    }

    // Makes an HTTP GET request to BlockCypher and stores the response as a snapshot in DB
    [HttpPost("{chain}/sync")]
    public async Task<IActionResult> Sync(string chain, CancellationToken ct)
    {
        if (!Enum.TryParse<BlockchainType>(chain, true, out var parsed))
            return BadRequest("Invalid blockchain. Use ETH_MAIN, DASH_MAIN, BTC_MAIN, BTC_TEST3, LTC_MAIN");

        _logger.LogInformation("Sync requested for {Chain}", parsed);
        var id = await _service.FetchAndStoreAsync(parsed, ct);
        return Ok(new { SnapshotId = id, Chain = parsed.ToString() });
    }

    // Syncs ALL supported blockchains (parallel fetch, safe persistence)
    [HttpPost("sync")]
    public async Task<IActionResult> SyncAll(CancellationToken ct)
    {
        var ids = await _service.FetchAndStoreAllAsync(ct);
        return Ok(new { Count = ids.Count, SnapshotIds = ids });
    }

    // Returns the stored history for a blockchain ordered by CreatedAt DESC
    [HttpGet("{chain}/history")]
    public async Task<IActionResult> History(
    string chain,
    [FromQuery] PagingQuery query,
    CancellationToken ct = default)
    {
        if (!Enum.TryParse<BlockchainType>(chain, true, out var parsed))
            return BadRequest("Invalid blockchain.");

        query.Page = Math.Max(query.Page, 1);
        query.PageSize = Math.Clamp(query.PageSize, 1, 200);

        _logger.LogInformation("History requested for {Chain}. Page={Page} PageSize={PageSize}", parsed, query.Page, query.PageSize);

        var rows = await _repository.GetHistoryAsync(parsed.ToString(), query.Page, query.PageSize, ct);
        return Ok(rows.Select(x => x.ToDto()));
    }

    // Returns the latest stored snapshot for a blockchain
    [HttpGet("{chain}/latest")]
    public async Task<IActionResult> Latest(string chain, CancellationToken ct)
    {
        if (!Enum.TryParse<BlockchainType>(chain, true, out var parsed))
            return BadRequest("Invalid blockchain.");

        var row = await _repository.GetLatestAsync(parsed.ToString(), ct);
        if (row is null) return NotFound();
        return Ok(row.ToDto());
    }
}
