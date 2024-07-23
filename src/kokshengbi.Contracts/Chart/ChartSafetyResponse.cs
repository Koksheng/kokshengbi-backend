namespace kokshengbi.Contracts.Chart
{
    public record ChartSafetyResponse(
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
        string chartType,

        /**
         * 生成的图表信息
         */
        string genChart,

        /**
         * 生成的分析结论
         */
        string genResult,

        /**
         * 图表状态 wait-等待,running-生成中,succeed-成功生成,failed-生成失败
         */
        string status,

        /**
         * 执行信息
         */
        string execMessage,

        /**
         * 创建图标用户 id
         */
        int userId,
        DateTime createTime,
        DateTime updateTime,
        int isDelete
        );
}
