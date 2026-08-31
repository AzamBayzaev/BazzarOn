namespace BazzarOn.Mediator.Helper.Common.Models;

/// <summary>
/// Параметры постраничной выборки.
/// </summary>
/// <param name="Index">Номер страницы, начиная с 0 (0 — первая страница).</param>
/// <param name="Size">Количество элементов на странице.</param>
public sealed record PaginationInfo(int Index, int Size);
