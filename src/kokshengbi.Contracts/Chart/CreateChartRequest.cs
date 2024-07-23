namespace kokshengbi.Contracts.Chart
{
    public record CreateChartRequest(
        string chartName, string goal, string chartData, string chartType
        );
}
