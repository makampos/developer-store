using System.Collections.ObjectModel;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class PagedResult<T>
{
    public PagedResult()
    {
        Items = ReadOnlyCollection<T>.Empty;
        TotalCount = 0;
        PageSize = 0;
        CurrentPage = 0;
    }
    private PagedResult(IReadOnlyList<T>? items, int totalCount, int pageSize, int currentPage)
    {
        Items = items;
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
    }

    public IReadOnlyList<T>? Items { get;  set; }
    public int TotalCount { get;  set; }
    public int PageSize { get;  set; }
    public int CurrentPage { get;  set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public static PagedResult<T> Create(IReadOnlyList<T>? items, int totalCount, int pageSize, int currentPage)
    {
        return new PagedResult<T>(items, totalCount, pageSize, currentPage);
    }
}