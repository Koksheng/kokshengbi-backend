namespace kokshengbi.Contracts.Chart
{
    public record QueryChartRequest(
        int? id,
        string? goal,
        string? chartName,
        string? chartType,
        int? userId,
        int? current,
        int? pageSize,
        string? sortField,
        string? sortOrder
        );
}
