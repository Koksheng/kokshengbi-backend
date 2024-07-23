using AutoMapper;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Interfaces.Services;
using kokshengbi.Application.Common.Models;
using kokshengbi.Application.Common.Utils;
using kokshengbi.Domain.ChartAggregate;
using MediatR;

namespace kokshengbi.Application.Charts.Commands.CreateChart
{
    public class CreateChartCommandHandler :
        IRequestHandler<CreateChartCommand, BaseResponse<int>>
    {
        private readonly IChartRepository _chartRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public CreateChartCommandHandler(IChartRepository chartRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _chartRepository = chartRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponse<int>> Handle(CreateChartCommand command, CancellationToken cancellationToken)
        {
            string userState = command.userState;

            // 1. Verify User using userId in userState
            var safetyUser = await _currentUserService.GetCurrentUserAsync(userState);

            // 2. Map Command to Chart
            Chart chart = _mapper.Map<Chart>(command);
            chart.userId = safetyUser.Id;
            chart.createTime = DateTime.Now;
            chart.updateTime = DateTime.Now;
            chart.status = "wait";

            // 3. Persist Chart
            var result = await _chartRepository.Add(chart);

            // 4. Return Chart ID
            if (result == 1)
            {
                return ResultUtils.success(data: chart.Id.Value);
            }
            else
            {
                throw new BusinessException(ErrorCode.OPERATION_ERROR);
            }
        }
    }
}
