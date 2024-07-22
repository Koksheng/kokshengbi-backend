using AutoMapper;
using kokshengbi.Application.Common.Constants;
using kokshengbi.Application.Common.Exceptions;
using kokshengbi.Application.Common.Models;
using kokshengbi.Application.Common.Utils;
using kokshengbi.Application.Users.Commands.Register;
using kokshengbi.Application.Users.Queries.GetCurrentUser;
using kokshengbi.Application.Users.Queries.Login;
using kokshengbi.Contracts.User;
using kokshengbi.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace kokshengbi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _mediator;
        //private const string USER_LOGIN_STATE = "userLoginState";
        private readonly IMapper _mapper;

        public UserController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponse<int>> userRegister(UserRegisterRequest request)
        {
            var command = _mapper.Map<UserRegisterCommand>(request);
            return await _mediator.Send(command);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponse<UserSafetyResponse>?> userLogin(UserLoginRequest request)
        {
            //var query = new UserLoginQuery(request.userAccount, request.userPassword);
            var query = _mapper.Map<UserLoginQuery>(request);
            var safetyUser = await _mediator.Send(query);

            // Convert user object to JSON string
            var serializedSafetyUser = JsonConvert.SerializeObject(safetyUser);

            // add user into session
            if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString(ApplicationConstants.USER_LOGIN_STATE)))
            {
                HttpContext.Session.SetString(ApplicationConstants.USER_LOGIN_STATE, serializedSafetyUser);
            }

            // map UserSafetyResult to UserSafetyResponse
            var response = _mapper.Map<UserSafetyResponse>(safetyUser);
            return ResultUtils.success(response);
        }

        [HttpPost]
        //[Authorize]
        public async Task<BaseResponse<int>> userLogout()
        {
            var userState = HttpContext.Session.GetString(ApplicationConstants.USER_LOGIN_STATE);
            if (string.IsNullOrWhiteSpace(userState))
            {
                throw new BusinessException(ErrorCode.PARAMS_ERROR, "session里找不到用户状态");
            }
            HttpContext.Session.Remove(ApplicationConstants.USER_LOGIN_STATE);
            //return 1;
            return ResultUtils.success(1);
        }

        [HttpGet]
        //[Authorize]
        public async Task<BaseResponse<UserSafetyResponse>?> getCurrentUser()
        {
            var userState = HttpContext.Session.GetString(ApplicationConstants.USER_LOGIN_STATE);

            var query = new GetCurrentUserQuery(userState);
            var currentSafetyUser = await _mediator.Send(query);

            // map UserSafetyResult to UserSafetyResponse
            var response = _mapper.Map<UserSafetyResponse>(currentSafetyUser);

            return ResultUtils.success(response);
        }
    }
}
