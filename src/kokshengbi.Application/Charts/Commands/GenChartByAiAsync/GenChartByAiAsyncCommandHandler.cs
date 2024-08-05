﻿using AutoMapper;
using kokshengbi.Application.Charts.Commands.GenChartByAi;
using kokshengbi.Application.Charts.Common;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Interfaces.Services;
using kokshengbi.Application.Common.Utils;
using kokshengbi.Contracts.Chart;
using kokshengbi.Domain.ChartAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kokshengbi.Application.Charts.Commands.GenChartByAiAsync
{
    public class GenChartByAiAsyncCommandHandler :
        IRequestHandler<GenChartByAiAsyncCommand, BIResult>
    {
        private readonly IChartRepository _chartRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IExcelService _excelService;
        private readonly IOpenAiService _openAiService;
        //private readonly IRedisRateLimiterService _rateLimiterService; // Comment this on office pc, cause dont have Redis 
        private readonly IThreadPoolService _threadPoolService;

        public GenChartByAiAsyncCommandHandler(IChartRepository chartRepository, ICurrentUserService currentUserService, IMapper mapper, IExcelService excelService, IOpenAiService openAiService, IThreadPoolService threadPoolService)
        {
            _chartRepository = chartRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _excelService = excelService;
            _openAiService = openAiService;
            _threadPoolService = threadPoolService;
            //_rateLimiterService = rateLimiterService; // Comment this on office pc, cause dont have Redis 
        }
        public async Task<BIResult> Handle(GenChartByAiAsyncCommand command, CancellationToken cancellationToken)
        {
            //int id = command.id;
            string chartName = command.chartName;
            string goal = command.goal;
            string chartType = command.chartType;
            string userState = command.userState;
            var file = command.file;

            // Verify User using userId in userState
            var safetyUser = await _currentUserService.GetCurrentUserAsync(userState);

            //// Limit each user to 2 requests per second // Comment this on office pc, cause dont have Redis 
            //if (!await _rateLimiterService.IsAllowedAsync($"genChartByAi_{safetyUser.Id}:{safetyUser.Id}", 2, TimeSpan.FromSeconds(1)))
            //{
            //    throw new BusinessException(ErrorCode.TOO_MANY_REQUEST, "You have exceeded the request limit.");
            //}

            // 校验文件 Validate command.file
            if (file == null)
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR, "File is empty");
            }

            // 校验文件大小 Validate file size
            const long ONE_MB = 1024 * 1024;
            if (file.Length > ONE_MB)
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR, "文件超过 1M, Files larger than 1M");
            }

            // 校验文件后缀 aaa.png Validate file extension
            string originalFilename = file.FileName;
            string suffix = Path.GetExtension(originalFilename).TrimStart('.').ToLower();
            var validFileSuffixList = new List<string> { "xlsx" };

            if (!validFileSuffixList.Contains(suffix))
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR, "文件后缀非法, Illegal file extension");
            }


            // 对内容进行压缩
            var csvData = await _excelService.ConvertExcelToCsvAsync(file);

            // 用户输入
            StringBuilder userInput = new StringBuilder();
            // 压缩后的数据
            userInput.Append("Data in csv separated with comma:").Append("\n").Append(csvData);
            //userInput.Append("Chart Type：").Append("Bar Chart").Append(". \n");
            //userInput.Append("Requirement：").Append("You are a Data Analyst now. Please analyze the data with the chart type").Append(". \n");

            if (!string.IsNullOrEmpty(chartType))
            {
                userInput.Append("Chart Type：").Append(chartType).Append(". \n");
            }

            userInput.Append("Chart Name：").Append(chartName).Append(". \n")
            .Append("Requirement：").Append("You are a Data Analyst now. ").Append(goal).Append(". \n")

            .Append("Generate a response based on:").Append("\n")
            .Append("1. Echarts V5 in Json string for source of Echarts generation, set chartName as Echarts's title and chartType as Echarts's type (no comments). Ensure the JSON is correctly formatted for the specified chart type. For example:\n")
            .Append("   - For line charts: { title: { text: 'Chart Name' }, xAxis: { type: 'category', data: [...] }, yAxis: { type: 'value' }, series: [ { data: [...], type: 'line' } ] }\n")
            .Append("   - For pie charts: { title: { text: 'Chart Name', left: 'center' }, tooltip: { trigger: 'item' }, legend: { orient: 'vertical', left: 'left' }, series: [ { name: 'Access From', type: 'pie', radius: '50%', data: [...], emphasis: { itemStyle: { shadowBlur: 10, shadowOffsetX: 0, shadowColor: 'rgba(0, 0, 0, 0.5)' } } } ] }\n")
            .Append("   - For radar charts: { title: { text: 'Chart Name' }, legend: { data: ['Allocated Budget', 'Actual Spending'] }, radar: { indicator: [ { name: 'Sales', max: 6500 }, { name: 'Administration', max: 16000 }, { name: 'Information Technology', max: 30000 }, { name: 'Customer Support', max: 38000 }, { name: 'Development', max: 52000 }, { name: 'Marketing', max: 25000 } ] }, series: [ { name: 'Budget vs spending', type: 'radar', data: [ { value: [...], name: 'Allocated Budget' }, { value: [...], name: 'Actual Spending' } ] } ] }\n")
            .Append("2. Detailed analysis conclusions (no comments).").Append("\n")

            //Expected Result (must use this one, if not the response key will name as echrtsCode and analysis)
            .Append("Here is an example of expected response format. Please follow this format strictly.").Append("\n\n")
            .Append("Echart:").Append("\n")
            .Append("{ title: { text: 'Chart Name' }, xAxis: { type: 'category', data: ['1', '2', '3'] }, yAxis: { type: 'value' }, series: [ { data: [10, 20, 30], type: 'line' } ]};").Append("\n")
            .Append("Conclusion:").Append("\n")
            .Append("Based on the data analysis, the number of users shows a consistent increase over the three days. The number of users doubled from day 1 to day 2 and increased by 10 users each day, indicating a steady growth trend.").Append("\n");


            // Insert the initial Chart record to database

            GenChartByAiAsyncChartInit genChartByAiAsyncChartInit = new GenChartByAiAsyncChartInit
            {
                goal = goal,
                chartName = chartName,
                chartData = csvData,
                chartType = chartType,
                status = "wait",
            };
            Chart chart = _mapper.Map<Chart>(genChartByAiAsyncChartInit);
            chart.userId = safetyUser.Id;
            chart.createTime = DateTime.Now;
            chart.updateTime = DateTime.Now;

            var result = await _chartRepository.Add(chart);
            if (result != 1)
            {
                throw new BusinessException(ErrorCode.OPERATION_ERROR);
            }

            // Queue the background work
            _threadPoolService.QueueBackgroundWorkItem(async cancellationToken =>
            {
                try
                {
                    // Update status to 'running'
                    chart.status = "running";
                    await _chartRepository.Update(chart);

                    // Generate AI chart and analysis
                    var openAiResponse = await _openAiService.GenerateTextAsync(userInput.ToString());
                    var parsedResponse = OpenAiResponseParser.ParseOpenAiResponse(openAiResponse);

                    // Update chart with the generated data
                    chart.genChart = (string?)parsedResponse.echart;
                    chart.genResult = parsedResponse.conclusion;
                    chart.status = "succeed";
                    await _chartRepository.Update(chart);
                }
                catch (Exception ex)
                {
                    chart.status = "failed";
                    chart.execMessage = ex.Message;
                    await _chartRepository.Update(chart);
                }
            });



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