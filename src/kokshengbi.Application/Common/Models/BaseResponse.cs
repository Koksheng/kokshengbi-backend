namespace kokshengbi.Application.Common.Models
{
    public record BaseResponse<T>(int code, T data, string message = "", string description = "")
    {

    };
}
