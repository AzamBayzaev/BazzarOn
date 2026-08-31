namespace BazzarOn.Mediator.Helper.Common.Models;

public sealed record PaginatedResult<T>(IReadOnlyList<T> Items, long TotalCount);
