using kokshengbi.Application.Users.Common;
using MediatR;

namespace kokshengbi.Application.Users.Queries.Login
{
    public record UserLoginQuery(string userAccount, string userPassword) : IRequest<UserSafetyResult?>;
}
