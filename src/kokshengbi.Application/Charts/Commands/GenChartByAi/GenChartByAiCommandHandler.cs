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
        private readonly IOpenAiService _openAiService;

        public GenChartByAiCommandHandler(IChartRepository chartRepository, ICurrentUserService currentUserService, IMapper mapper, IExcelService excelService, IOpenAiService openAiService)
        {
            _chartRepository = chartRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _excelService = excelService;
            _openAiService = openAiService;
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
            //userInput.Append("分析需求：").Append(goal).Append("\n");
            userInput.Append("分析需求：").Append("分析网站用户的增长情况").Append("\n");
            userInput.Append("请使用：").Append("line chart").Append("\n");
            // 压缩后的数据
            userInput.Append("原始数据：").Append(csvData).Append("\n");
            userInput.Append("请根据以上内容，按照以下指定格式生成内容（此外不要输出任何多余的开头、结尾、注释）").Append("\n");
            userInput.Append("【【【【【\n");
            userInput.Append("{前端 Echarts V5 的 option 配置对象js代码，合理地将数据进行可视化，不要生成任何多余的内容，比如注释}\n");
            userInput.Append("【【【【【\n");
            userInput.Append("{明确的数据分析结论、越详细越好，不要生成多余的注释}");

            // Now I wan to call the https://api.yucongming.com/api/dev

            //var aiResponse = await _openAiClient.GenerateTextAsync(userInput.ToString());

            var openAiResponse = await _openAiService.GenerateTextAsync(userInput.ToString());


            return ResultUtils.success(userInput.ToString());
        }
    }
}
