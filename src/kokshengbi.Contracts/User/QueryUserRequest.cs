namespace kokshengbi.Contracts.User
{
    public record QueryUserRequest(
        int? id,
        string? userName,
        string? userAccount,
        //string? userAvatar,
        int? gender,
        string? userRole,
        int? isDelete,
        int? current,
        int? pageSize,
        string? sortField,
        string? sortOrder
        );
}
