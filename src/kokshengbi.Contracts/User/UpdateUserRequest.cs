namespace kokshengbi.Contracts.User
{
    public record UpdateUserRequest(
        int id,
        string userName,
        //string userAccount,
        string userAvatar,
        int gender,
        string userRole,
        int isDelete
        );
}
