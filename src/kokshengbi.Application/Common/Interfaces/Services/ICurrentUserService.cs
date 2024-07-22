using kokshengbi.Application.Users.Common;

namespace kokshengbi.Application.Common.Interfaces.Services
{
    public interface ICurrentUserService
    {
        Task<UserSafetyResult> GetCurrentUserAsync(string userState);
        Task<bool> IsAdminAsync(UserSafetyResult safetyUser);
    }
}
