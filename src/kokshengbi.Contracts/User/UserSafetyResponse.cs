namespace kokshengbi.Contracts.User
{
    public record UserSafetyResponse(
        int Id,
        string userName,
        string userAccount,
        string userAvatar,
        int gender,
        string userRole,
        DateTime createTime,
        DateTime updateTime,
        string token);
}
