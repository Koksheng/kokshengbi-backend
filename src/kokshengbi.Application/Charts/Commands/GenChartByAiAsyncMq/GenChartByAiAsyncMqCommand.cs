using kokshengbi.Application.Charts.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace kokshengbi.Application.Charts.Commands.GenChartByAiAsyncMq
{
    public class GenChartByAiAsyncMqCommand : IRequest<BIResult>
    {
        public string chartName { get; set; }
        public string goal { get; set; }
        public string chartType { get; set; }
        public string userState { get; set; }
        public IFormFile file { get; set; }
    }
}
