using kokshengbi.Application.Common.Models;
using MediatR;

namespace kokshengbi.Application.Charts.Commands.CreateChart
{
    public record CreateChartCommand
    (string chartName, string goal, string chartData, string chartType, string userState) : IRequest<BaseResponse<int>>;
}
