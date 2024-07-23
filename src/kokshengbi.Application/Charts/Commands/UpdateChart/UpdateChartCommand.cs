using kokshengbi.Application.Common.Models;
using MediatR;

namespace kokshengbi.Application.Charts.Commands.UpdateChart
{
    public record UpdateChartCommand(
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
         * 创建图标用户 id
         */
        int userId,
        int isDelete,
        string userState
        ) : IRequest<BaseResponse<int>>;
}
