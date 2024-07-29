using AutoMapper;
using kokshengbi.Application.Charts.Commands.CreateChart;
using kokshengbi.Application.Charts.Commands.DeleteChart;
using kokshengbi.Application.Charts.Commands.GenChartByAi;
using kokshengbi.Application.Charts.Commands.UpdateChart;
using kokshengbi.Application.Charts.Queries.GetChartById;
using kokshengbi.Application.Charts.Queries.ListChartByPage;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Models;
using kokshengbi.Application.Common.Utils;
using kokshengbi.Contracts.Chart;
using kokshengbi.Contracts.Common;
using kokshengbi.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace kokshengbi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public ChartController(IMapper mapper, ISender mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<BaseResponse<int>> addChart(CreateChartRequest request)
        {
            if (request == null)
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR);
            }

            var userState = HttpContext.Session.GetString(ApplicationConstants.USER_LOGIN_STATE);

            var command = _mapper.Map<CreateChartCommand>(request);
            // Assign the userId
            command = command with { userState = userState };
            return await _mediator.Send(command);
        }

        [HttpPost]
        public async Task<BaseResponse<int>> deleteChart(DeleteRequest request)
        {
            if (request == null || request.id <= 0)
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR);
            }

            var userState = HttpContext.Session.GetString(ApplicationConstants.USER_LOGIN_STATE);

            var command = _mapper.Map<DeleteChartCommand>(request);
            // Assign the userState
            command = command with { userState = userState };
            return await _mediator.Send(command);
        }

        [HttpPost]
        public async Task<BaseResponse<int>> updateChart(UpdateChartRequest request)
        {
            if (request == null)
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR);
            }

            var userState = HttpContext.Session.GetString(ApplicationConstants.USER_LOGIN_STATE);

            var command = _mapper.Map<UpdateChartCommand>(request);
            // Assign the userState
            command = command with { userState = userState };
            return await _mediator.Send(command);
        }

        [HttpGet]
        public async Task<BaseResponse<ChartSafetyResponse>> getChartById(int id)
        {
            var userState = HttpContext.Session.GetString(ApplicationConstants.USER_LOGIN_STATE);

            var query = new GetChartByIdQuery(id, userState);

            var result = await _mediator.Send(query);

            // map result to response
            var response = _mapper.Map<ChartSafetyResponse>(result);

            return ResultUtils.success(response);
        }

        [HttpGet("list/page")]
        public async Task<BaseResponse<PaginatedList<ChartSafetyResponse>>> listChartByPage([FromQuery] QueryChartRequest request)
        {
            if (request == null)
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR);
            }

            var query = _mapper.Map<ListChartByPageQuery>(request);
            var result = await _mediator.Send(query);

            var response = _mapper.Map<PaginatedList<ChartSafetyResponse>>(result);

            return ResultUtils.success(response);
        }

        [HttpPost]
        public async Task<BaseResponse<BIResponse>> genChartByAi([FromForm] GenChartByAiRequestWrapper requestWrapper)
        {

            if (requestWrapper.file == null || requestWrapper.file.Length == 0)
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR);
            }

            //var request = requestWrapper.request;
            //if (request == null)
            //{
            //    throw new BusinessException(ErrorCode.PARAMS_ERROR);
            //}

            var userState = HttpContext.Session.GetString(ApplicationConstants.USER_LOGIN_STATE);
            if (userState == null)
            {
                throw new BusinessException(ErrorCode.NOT_LOGIN);
            }

            // Create the command and include the file
            var command = new GenChartByAiCommand
            {
                chartName = requestWrapper.chartName,
                goal = requestWrapper.goal,
                chartType = requestWrapper.chartType,
                userState = userState,
                file = requestWrapper.file // Include the file here
            };

            // Send the command to the mediator
            var result = await _mediator.Send(command);

            // map result to response
            var response = _mapper.Map<BIResponse>(result);

            return ResultUtils.success(response);
        }
    }
}
