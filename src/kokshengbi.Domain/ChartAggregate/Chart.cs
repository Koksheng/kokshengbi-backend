using kokshengbi.Domain.ChartAggregate.ValueObjects;
using kokshengbi.Domain.Common.Models;

namespace kokshengbi.Domain.ChartAggregate
{
    public sealed class Chart : AggregateRoot<ChartId, int>
    {
        public string goal { get; set; }
        public string name { get; set; }
        public string chartData { get; set; }
        public string chartType { get; set; }
        public string genChart { get; set; }
        public string genResult { get; set; }
        public string status { get; set; }
        public string execMessage { get; set; }
        public int userId { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public int isDelete { get; set; }

        private Chart(
            ChartId chartId,
            string goal,
            string name,
            string chartData,
            string chartType,
            string genChart,
            string genResult,
            string status,
            string execMessage,
            int userId,
            DateTime createTime,
            DateTime updateTime,
            int isDelete)
            : base(chartId)
        {
            goal = goal;
            name = name;
            chartData = chartData;
            chartType = chartType;
            genChart = genChart;
            genResult = genResult;
            status = status;
            execMessage = execMessage;
            userId = userId;
            createTime = createTime;
            updateTime = updateTime;
            isDelete = isDelete;
        }

        public static Chart Create(
            string goal,
            string name,
            string chartData,
            string chartType,
            string genChart,
            string genResult,
            string status,
            string execMessage,
            int userId)
        {
            return new(
                null,  // EF Core will set this value
                goal,
                name,
                chartData,
                chartType,
                genChart,
                genResult,
                status,
                execMessage,
                userId,
                DateTime.UtcNow,
                DateTime.UtcNow,
                0);
        }
        // Private parameterless constructor for EF Core
        private Chart() : base(null)
        {
            // EF Core requires an empty constructor for materialization
        }
    }
}
