using AutoMapper;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Models;
using kokshengbi.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
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

        //[HttpPost]
        //public async Task<BaseResponse<int>> addChart(CreateChartRequest request)
        //{
        //    if (request == null)
        //    {
        //        throw new BusinessException(ErrorCode.PARAMS_ERROR);
        //    }

        //    var userState = HttpContext.Session.GetString(ApplicationConstants.USER_LOGIN_STATE);

        //    var command = _mapper.Map<CreateInterfaceInfoCommand>(request);
        //    // Assign the userId
        //    command = command with { userState = userState };
        //    return await _mediator.Send(command);
        //}
    }
}
