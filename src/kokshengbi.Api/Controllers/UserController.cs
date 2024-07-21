using AutoMapper;
using kokshengbi.Application.Common.Models;
using kokshengbi.Application.Users.Commands.Register;
using kokshengbi.Contracts.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kokshengbi.Api.Controllers
{
    [Route("api/[controller]")]
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
            //var command = new UserRegisterCommand(request.userAccount, request.userPassword, request.checkPassword);
            var command = _mapper.Map<UserRegisterCommand>(request);
            return await _mediator.Send(command);
            //return await _userService.UserRegister(request);
        }
    }
}
