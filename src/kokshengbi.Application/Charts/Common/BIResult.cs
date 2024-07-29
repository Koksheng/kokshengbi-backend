namespace kokshengbi.Application.Charts.Common
{
    public record BIResult
    {
        public string GenChart { get; init; }
        public string GenResult { get; init; }
        public int ChartId { get; init; }

        public BIResult() { }

        public BIResult(string genChart, string genResult, int chartId)
        {
            GenChart = genChart;
            GenResult = genResult;
            ChartId = chartId;
        }
    }
}
