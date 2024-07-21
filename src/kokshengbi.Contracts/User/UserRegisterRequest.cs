namespace kokshengbi.Contracts.User
{
    public record UserRegisterRequest(string userAccount, string userPassword, string checkPassword);
}
