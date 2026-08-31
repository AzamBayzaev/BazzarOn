using Ardalis.Specification;
using BazzarOn.Mediator.Helper.Common.Models;

namespace BazzarOn.Mediator.Helper.Common.Extensions;

public static class PaginationExtensions
{
    public static ISpecificationBuilder<T> WithPagination<T>(
        this ISpecificationBuilder<T> builder,
        PaginationInfo paginationInfo)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(paginationInfo);

        if (paginationInfo.Index < 0)
            throw new ArgumentOutOfRangeException(
                nameof(paginationInfo), paginationInfo.Index, "Index страницы не может быть отрицательным.");

        if (paginationInfo.Size <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(paginationInfo), paginationInfo.Size, "Size страницы должен быть больше нуля.");

        return builder
            .Skip(paginationInfo.Size * paginationInfo.Index)
            .Take(paginationInfo.Size);
    }
}