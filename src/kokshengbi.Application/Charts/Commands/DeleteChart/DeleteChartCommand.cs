using kokshengbi.Application.Common.Models;
using MediatR;

namespace kokshengbi.Application.Charts.Commands.DeleteChart
{
    public record DeleteChartCommand(int id, string userState) : IRequest<BaseResponse<int>>;
}
