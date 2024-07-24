namespace kokshengbi.Contracts.Chart
{
    public record UpdateChartRequest(
        int id,
        /**
         * 分析目标
         */
        string goal,

        /**
         * 图表信息
         */
        string chartData,

        /**
         * 图表名称
         */
        string chartName,

        /**
         * 图表类型
         */
        string chartType

        /**
         * 生成的图表信息
         */
        //string genChart,

        /**
         * 生成的分析结论
         */
        //string genResult,

        /**
         * 创建图标用户 id
         */
        //int userId,
        //int isDelete

        );
}
