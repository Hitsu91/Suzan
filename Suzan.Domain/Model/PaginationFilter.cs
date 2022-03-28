using System.Text.Json.Serialization;

namespace Suzan.Domain.Model;

public class PaginationFilter
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = default;
    public Direction Direction { get; set; } = Direction.Asc;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Direction
{
    Asc,
    Desc
}
