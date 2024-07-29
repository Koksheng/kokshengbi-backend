using AutoMapper;
using kokshengbi.Application.Charts.Common;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Interfaces.Services;
using kokshengbi.Contracts.Chart;
using kokshengbi.Domain.ChartAggregate;
using MediatR;
using Newtonsoft.Json;
using System.Text;

namespace kokshengbi.Application.Charts.Commands.GenChartByAi
{
    public class GenChartByAiCommandHandler :
        IRequestHandler<GenChartByAiCommand, BIResult>
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
        public async Task<BIResult> Handle(GenChartByAiCommand command, CancellationToken cancellationToken)
        {
            //int id = command.id;
            string chartName = command.chartName;
            string goal = command.goal;
            string chartType = command.chartType;
            string userState = command.userState;

            // 1. Verify User using userId in userState
            var safetyUser = await _currentUserService.GetCurrentUserAsync(userState);

            // 对内容进行压缩
            var csvData = await _excelService.ConvertExcelToCsvAsync(command.file);

            //// 用户输入
            //StringBuilder userInput = new StringBuilder();
            ////userInput.Append("分析需求：").Append(goal).Append("\n");
            //userInput.Append("分析需求：").Append("分析网站用户的增长情况").Append("\n");
            //userInput.Append("请使用：").Append("line chart").Append("\n");
            //// 压缩后的数据
            //userInput.Append("原始数据：").Append(csvData).Append("\n");
            //userInput.Append("请根据以上内容，按照以下指定格式生成内容（此外不要输出任何多余的开头、结尾、注释）").Append("\n");
            //userInput.Append("【【【【【\n");
            //userInput.Append("{前端 Echarts V5 的 option 配置对象js代码，合理地将数据进行可视化，不要生成任何多余的内容，比如注释}\n");
            //userInput.Append("【【【【【\n");
            //userInput.Append("{明确的数据分析结论、越详细越好，不要生成多余的注释}");

            // 用户输入
            StringBuilder userInput = new StringBuilder();
            // 压缩后的数据
            userInput.Append("Data in csv separated with comma:").Append("\n").Append(csvData);
            //userInput.Append("Chart Type：").Append("Bar Chart").Append(". \n");
            //userInput.Append("Requirement：").Append("You are a Data Analyst now. Please analyze the data with the chart type above").Append(". \n");
            
            if(!string.IsNullOrEmpty(chartType))
            {
                userInput.Append("Chart Type：").Append(chartType).Append(". \n");
            }
            
            userInput.Append("Requirement：").Append("You are a Data Analyst now. ").Append(goal).Append(". \n");

            userInput.Append("Generate a response based on:").Append("\n");
            userInput.Append("1. Echarts V5 in JS code for source of Echarts generation (no comments).\n");
            userInput.Append("2. Detailed analysis conclusions (no comments).").Append(". \n");

            //Expected Result (must use this one, if not the response key will name as echrtsCode and analysis)
            userInput.Append("Here is an example of expected response format. Please follow this format strictly.").Append("\n\n");
            userInput.Append("Echart:").Append("\n");
            userInput.Append("option = { xAxis: { type: 'category', data: ['1', '2', '3'] }, yAxis: { type: 'value' }, series: [ { data: [10, 20, 30], type: 'line' } ]};").Append("\n");
            userInput.Append("Conclusion:").Append("\n");
            userInput.Append("Based on the data analysis, the number of users shows a consistent increase over the three days. The number of users doubled from day 1 to day 2 and increased by 10 users each day, indicating a steady growth trend.").Append("\n");


            var openAiResponse = await _openAiService.GenerateTextAsync(userInput.ToString());
            // Parse the JSON response using Newtonsoft.Json
            var parsedResponse = JsonConvert.DeserializeObject<OpenAIApiResponse>(openAiResponse);

            // 插入到数据库
            Chart chart = _mapper.Map<Chart>(parsedResponse);
            chart.goal = goal;
            chart.chartName = chartName;
            chart.chartData = csvData;
            chart.chartType = chartType;
            chart.status = "wait";
            //chart.execMessage = "";
            chart.userId = safetyUser.Id;
            chart.createTime = DateTime.Now;
            chart.updateTime = DateTime.Now;
            //chart.isDelete = 0;

            // Persist Chart
            var result = await _chartRepository.Add(chart);

            // Return BIResult
            if (result == 1)
            {
                return _mapper.Map<BIResult>(chart);
            }
            else
            {
                throw new BusinessException(ErrorCode.OPERATION_ERROR);
            }
        }
    }
}
