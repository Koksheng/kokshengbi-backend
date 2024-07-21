using kokshengbi.Application.Common.Models;
using MediatR;

namespace kokshengbi.Application.Users.Commands.Register
{
    public record UserRegisterCommand(string userAccount, string userPassword, string checkPassword) : IRequest<BaseResponse<int>>;
}
