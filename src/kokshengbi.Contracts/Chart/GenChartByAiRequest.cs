namespace kokshengbi.Contracts.Chart
{
    public record GenChartByAiRequest(

        /**
         * 图表名称
         */
        string chartName,

        /**
         * 分析目标
         */
        string goal,

        /**
         * 图表类型
         */
        string chartType
        );
}
