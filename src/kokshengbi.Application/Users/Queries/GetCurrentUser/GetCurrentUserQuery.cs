using kokshengbi.Application.Users.Common;
using MediatR;

namespace kokshengbi.Application.Users.Queries.GetCurrentUser
{
    public record GetCurrentUserQuery(string userState) : IRequest<UserSafetyResult>;
}
