namespace kokshengbi.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(int userId, string userName, string userRole);
    }
}
