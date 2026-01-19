using System.ComponentModel.DataAnnotations;

namespace BlockChain.Api.Models;

public sealed class PagingQuery
{
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;

    [Range(1, 200)]
    public int PageSize { get; set; } = 50;
}