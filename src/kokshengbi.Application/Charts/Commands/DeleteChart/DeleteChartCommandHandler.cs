using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Interfaces.Persistence;
using kokshengbi.Application.Common.Interfaces.Services;
using kokshengbi.Application.Common.Models;
using kokshengbi.Application.Common.Utils;
using kokshengbi.Domain.ChartAggregate;
using MediatR;

namespace kokshengbi.Application.Charts.Commands.DeleteChart
{
    public class DeleteChartCommandHandler :
        IRequestHandler<DeleteChartCommand, BaseResponse<int>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IChartRepository _chartRepository;
        public DeleteChartCommandHandler(ICurrentUserService currentUserService, IChartRepository chartRepository)
        {
            _currentUserService = currentUserService;
            _chartRepository = chartRepository;
        }

        public async Task<BaseResponse<int>> Handle(DeleteChartCommand command, CancellationToken cancellationToken)
        {
            int id = command.id;
            string userState = command.userState;

            // 1. Verify User using userId in userState
            var safetyUser = await _currentUserService.GetCurrentUserAsync(userState);

            // 2. get the to be deleted item using id
            Chart oldchart = await _chartRepository.GetById(id);
            if (oldchart == null)
            {
                throw new BusinessException(ErrorCode.NOT_FOUND_ERROR, "Chart not found.");
            }

            // 3. Only the same userId or admin is allowed to delete
            if (!oldchart.userId.Equals(safetyUser.Id))
            {
                var isAdmin = await _currentUserService.IsAdminAsync(safetyUser);
                if (!isAdmin)
                {
                    throw new BusinessException(ErrorCode.NO_AUTH_ERROR, "User does not have permission to delete this chart.");
                }
            }

            int result = await _chartRepository.DeleteById(id);

            if (result == 0)
                throw new BusinessException(ErrorCode.SYSTEM_ERROR, "删除失败，数据库错误");


            return ResultUtils.success(data: id);
        }
    }
}
