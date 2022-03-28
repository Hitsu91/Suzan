namespace Suzan.Domain.Model;

public class PagedResponse<T>
{
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages { get; init; }
    public int TotalRecords { get; init; }
    public IEnumerable<T> Data { get; init; }

    public PagedResponse(IEnumerable<T> data, int pageNumber, int pageSize, int totalPages, int totalRecords)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = totalPages;
        TotalRecords = totalRecords;
        Data = data;
    }

    public PagedResponse<D> Select<D>(Func<T, D> selector)
    {
        return new PagedResponse<D>(Data.Select(selector).ToList(), PageNumber, PageSize, TotalPages, TotalRecords);
    }
}
