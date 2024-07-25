﻿using AutoMapper;
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

            // 对内容进行压缩
            var csvData = await _excelService.ConvertExcelToCsvAsync(command.file);

            // 用户输入
            StringBuilder userInput = new StringBuilder();
            userInput.Append("你是一个数据分析师，接下来我会给你我的分析目标和原始数据，请告诉我分析结论。").Append("\n");
            userInput.Append("分析目标：").Append(goal).Append("\n");
            // 压缩后的数据
            userInput.Append("数据：").Append(csvData).Append("\n");
            return ResultUtils.success(userInput.ToString());


            //// 2. get the to be deleted item using id
            //Chart oldChart = await _chartRepository.GetById(id);
            //if (oldChart == null)
            //{
            //    throw new BusinessException(ErrorCode.NOT_FOUND_ERROR, "Chart not found.");
            //}

            //// 3. Only the same userId or admin is allowed to delete
            //if (!oldChart.userId.Equals(safetyUser.Id))
            //{
            //    var isAdmin = await _currentUserService.IsAdminAsync(safetyUser);
            //    if (!isAdmin)
            //    {
            //        throw new BusinessException(ErrorCode.NO_AUTH_ERROR, "User does not have permission to update this chart.");
            //    }
            //}

            //// 4. Map the updated data to the existing entity
            //_mapper.Map(command, oldChart);
            //oldChart.updateTime = DateTime.Now;

            //// 5. Persist the updated entity
            //var result = await _chartRepository.Update(oldChart);

            //if (result == 1)
            //{
            //    return ResultUtils.success(data: oldChart.Id.Value);
            //}
            //else
            //{
            //    throw new BusinessException(ErrorCode.OPERATION_ERROR);
            //}
            //return ResultUtils.success(1);
        }
    }
}