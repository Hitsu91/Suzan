using System.Linq.Expressions;
using Suzan.Domain.Model;

namespace Suzan.Application.Helpers;

public static class PaginationHelper
{
    public static PagedResponse<T> CreatePagedResponse<T>(
        IEnumerable<T> pagedData, PaginationFilter paginationFilter, int totalRecords)
    {
        var totalPages = totalRecords / (double)paginationFilter.PageSize;
        var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        var response = new PagedResponse<T>(
            pagedData,
            paginationFilter.PageNumber,
            paginationFilter.PageSize,
            roundedTotalPages,
            totalRecords
        );

        return response;
    }

    public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> source,
        PaginationFilter paginationFilter)
    {
        if (paginationFilter.SortBy is not null)
            source = paginationFilter.Direction switch
            {
                Direction.Asc => source.OrderBy(paginationFilter.SortBy),
                Direction.Desc => source.OrderByDescending(paginationFilter.SortBy),
                _ => source
            };

        return source
            .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
            .Take(paginationFilter.PageSize);
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderBy(ToLambda<T>(propertyName));
    }

    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    {
        return source.OrderByDescending(ToLambda<T>(propertyName));
    }

    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T));
        var property = Expression.Property(parameter, propertyName);
        var propAsObject = Expression.Convert(property, typeof(object));

        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    }
}