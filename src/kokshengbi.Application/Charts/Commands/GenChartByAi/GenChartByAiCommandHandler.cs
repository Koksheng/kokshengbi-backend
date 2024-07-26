using AutoMapper;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Interfaces.Services;
using kokshengbi.Application.Common.Models;
using kokshengbi.Application.Common.Utils;
using MediatR;
using System.Text;

namespace kokshengbi.Application.Charts.Commands.GenChartByAi
{
    public class GenChartByAiCommandHandler :
        IRequestHandler<GenChartByAiCommand, BaseResponse<string>>
    {
        private readonly IChartRepository _chartRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;

        public GenChartByAiCommandHandler(IChartRepository chartRepository, ICurrentUserService currentUserService, IMapper mapper, IExcelService excelService)
        {
            _chartRepository = chartRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _excelService = excelService;
        }
        public async Task<BaseResponse<string>> Handle(GenChartByAiCommand command, CancellationToken cancellationToken)
        {
            //int id = command.id;
            string chartName = command.chartName;
            string goal = command.goal;
            string chartType = command.chartType;
            string userState = command.userState;

            //// 1. Verify User using userId in userState
            //var safetyUser = await _currentUserService.GetCurrentUserAsync(userState);
            long biModelId = 1651468516836098050;

            // 对内容进行压缩
            var csvData = await _excelService.ConvertExcelToCsvAsync(command.file);

            // 用户输入
            StringBuilder userInput = new StringBuilder();
            userInput.Append("分析需求：").Append(goal).Append("\n");
            // 压缩后的数据
            userInput.Append("原始数据：").Append(csvData).Append("\n");

            // Now I wan to call the https://api.yucongming.com/api/dev

            //var aiResponse = await _openAiClient.GenerateTextAsync(userInput.ToString());


            return ResultUtils.success(userInput.ToString());
        }
    }
}
