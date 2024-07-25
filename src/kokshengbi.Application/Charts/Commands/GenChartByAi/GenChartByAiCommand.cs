using kokshengbi.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace kokshengbi.Application.Charts.Commands.GenChartByAi
{
    public class GenChartByAiCommand : IRequest<BaseResponse<int>>
    {
        public string chartName { get; set; }
        public string goal { get; set; }
        public string chartType { get; set; }
        public string userState { get; set; }
        public IFormFile file { get; set; }
    }

}
