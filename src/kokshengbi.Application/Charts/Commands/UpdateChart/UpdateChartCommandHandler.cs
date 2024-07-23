using AutoMapper;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Interfaces.Services;
using kokshengbi.Application.Common.Models;
using kokshengbi.Application.Common.Utils;
using kokshengbi.Domain.ChartAggregate;
using MediatR;

namespace kokshengbi.Application.Charts.Commands.UpdateChart
{
    public class UpdateChartCommandHandler :
        IRequestHandler<UpdateChartCommand, BaseResponse<int>>
    {
        private readonly IChartRepository _chartRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UpdateChartCommandHandler(IChartRepository chartRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _chartRepository = chartRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<int>> Handle(UpdateChartCommand command, CancellationToken cancellationToken)
        {
            int id = command.id;
            string userState = command.userState;

            // 1. Verify User using userId in userState
            var safetyUser = await _currentUserService.GetCurrentUserAsync(userState);

            // 2. get the to be deleted item using id
            Chart oldChart= await _chartRepository.GetById(id);
            if (oldChart == null)
            {
                throw new BusinessException(ErrorCode.NOT_FOUND_ERROR, "Chart not found.");
            }

            // 3. Only the same userId or admin is allowed to delete
            if (!oldChart.userId.Equals(safetyUser.Id))
            {
                var isAdmin = await _currentUserService.IsAdminAsync(safetyUser);
                if (!isAdmin)
                {
                    throw new BusinessException(ErrorCode.NO_AUTH_ERROR, "User does not have permission to update this chart.");
                }
            }

            // 4. Map the updated data to the existing entity
            _mapper.Map(command, oldChart);
            oldChart.updateTime = DateTime.Now;

            // 5. Persist the updated entity
            var result = await _chartRepository.Update(oldChart);

            if (result == 1)
            {
                return ResultUtils.success(data: oldChart.Id.Value);
            }
            else
            {
                throw new BusinessException(ErrorCode.OPERATION_ERROR);
            }
        }
    }
}
